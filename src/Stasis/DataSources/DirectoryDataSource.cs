using System.Collections.Generic;
using System.IO;

namespace Stasis.DataSources
{
    public class DirectoryDataSource : IDataSource
    {
        private readonly string _path;

        public DirectoryDataSource(string path)
        {
            _path = path;
        }

        public async IAsyncEnumerable<RawItem> GetItems()
        {
            var allFiles = Directory.GetFiles(_path, "*.*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                var fileDetails = new FileInfo(file);
                var relativePath = file.Replace(_path, "").Replace("\\", "/");
                var htmlPath = relativePath.Replace(fileDetails.Extension, ".html");

                var contents = await File.ReadAllBytesAsync(file);

                yield return new RawItem
                {
                    SourceKey = relativePath,
                    Content = contents,
                    ContentType = fileDetails.Extension
                };
            }
        }

        public static implicit operator DirectoryDataSource(DirectoryInfo input)
        {
            return new DirectoryDataSource(input.FullName);
        }
    }
}