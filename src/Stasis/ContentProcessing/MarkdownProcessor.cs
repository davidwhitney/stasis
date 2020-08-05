using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markdig;
using Stasis.ContentModel;

namespace Stasis.ContentProcessing
{
    public class MarkdownProcessor : IContentProcessor
    {
        private readonly MarkdownPipeline _pipeline;
        private readonly MarkdownUnpacker _unpacker;

        public MarkdownProcessor()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _unpacker = new MarkdownUnpacker();
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
            var unpacked = _unpacker.Unpack(content);

            var asString = Markdown.ToHtml(unpacked.Body, _pipeline);
            return Encoding.UTF8.GetBytes(asString);
        }
    }

    public class MarkdownUnpacker
    {
        public UnpackedMarkdown Unpack(string fileContents)
        {
            if (!fileContents.StartsWith("---"))
            {
                throw new System.NotImplementedException();
            }

            var skipFirstThreeDashes = fileContents.TrimStart((char) 65279, '-', '\r', '\n');
            var parts = skipFirstThreeDashes.Split("---", StringSplitOptions.RemoveEmptyEntries);
                
            var result = new UnpackedMarkdown
            {
                Body = parts[1].TrimStart()
            };

            var variables = parts[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var variable in variables)
            {
                var variableParts = variable.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var key = variableParts[0].Trim();
                var value = variableParts[1].Trim();
                result.FrontMatter[key] = value;
            }

            return result;
        }
    }

    public class UnpackedMarkdown
    {
        public Dictionary<string, string> FrontMatter { get; set; } = new Dictionary<string, string>();
        public string Body { get; set; } = "";
    }
}