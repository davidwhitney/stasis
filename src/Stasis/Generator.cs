using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using RazorEngine;
using RazorEngine.Templating;
using Encoding = System.Text.Encoding;

namespace Stasis
{
    public class Generator
    {
        public List<IDataSource> DataSources { get; } = new List<IDataSource>();
        public List<IContentProcessor> ContentProcessors { get; } = new List<IContentProcessor>();
        public IOutputDestination Output { get; set; } = new InMemoryOutputDestination();

        public Generator()
        {
            ContentProcessors.Add(new RazorProcessor());
            ContentProcessors.Add(new MarkdownProcessor());
        }

        public async Task Generate()
        {
            foreach (var source in DataSources)
            {
                await foreach (var item in source.GetItems())
                {
                    var processor = ContentProcessors.First(x => x.Supports(item));
                    var bytes = processor.Process(item);

                    Output.Save(item.DestinationKey, bytes);
                }
            }
        }

        public Generator AddDataSource(IDataSource source)
        {
            DataSources.Add(source);
            return this;
        }

        public Generator AddDataSource(string source)
        {
            if (Directory.Exists(source))
            {
                AddDataSource(new DirectoryDataSource(source));
            }
            else
            {
                throw new InvalidOperationException("Data source not recognised");
            }

            return this;
        }
    }

    public interface IDataSource
    {
        IAsyncEnumerable<Item> GetItems();
    }

    public class DirectoryDataSource : IDataSource
    {
        private readonly string _path;

        public DirectoryDataSource(string path)
        {
            _path = path;
        }

        public async IAsyncEnumerable<Item> GetItems()
        {
            var allFiles = Directory.GetFiles(_path, "*.*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                var fileDetails = new FileInfo(file);
                var relativePath = file.Replace(_path, "").Replace("\\", "/");
                var htmlPath = relativePath.Replace(fileDetails.Extension, ".html");

                var contents = await File.ReadAllBytesAsync(file);

                yield return new Item
                {
                    SourceKey = relativePath,
                    DestinationKey = htmlPath,
                    Content = contents
                };
            }
        }

        public static implicit operator DirectoryDataSource(DirectoryInfo input)
        {
            return new DirectoryDataSource(input.FullName);
        }
    }

    public class Item
    {
        public string SourceKey { get; set; }
        public string DestinationKey { get; set; }
        public byte[] Content { get; set; }
    }

    public interface IContentProcessor
    {
        bool Supports(Item item);
        byte[] Process(Item item);
    }

    public class RazorProcessor : IContentProcessor
    {
        public bool Supports(Item item)
        {
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);
            return fileDetails.Extension == ".cshtml";
        }

        public byte[] Process(Item item)
        {
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);
            var content = Encoding.Default.GetString(item.Content);

            var result = Engine
                .Razor
                .RunCompile(content,
                    fileDetails.Name,
                    null,
                    new
                    {
                        Name = "World"
                    });

            var asBytes = System.Text.Encoding.UTF8.GetBytes(result);
            return asBytes;
        }
    }
    
    public class MarkdownProcessor : IContentProcessor
    {
        private MarkdownPipeline _pipeline;

        public MarkdownProcessor()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public bool Supports(Item item)
        {
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);
            return fileDetails.Extension == ".md";
        }

        public byte[] Process(Item item)
        {
            var content = Encoding.Default.GetString(item.Content);
            var asString = Markdown.ToHtml(content, _pipeline);
            return Encoding.UTF8.GetBytes(asString);
        }
    }

    public class InMemoryOutputDestination : IOutputDestination
    {
        public ConcurrentDictionary<string, byte[]> Files { get; set; } = new ConcurrentDictionary<string, byte[]>();
        public void Save(string path, byte[] contents) => Files[path] = contents;
    }

    public interface IOutputDestination
    {
        void Save(string path, byte[] contents);
    }
}
