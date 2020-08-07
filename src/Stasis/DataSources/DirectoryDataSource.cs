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
                var contents = await File.ReadAllBytesAsync(file);
                yield return new LocalFileSystemItem(_path, file, contents);
            }
        }

        public static implicit operator DirectoryDataSource(DirectoryInfo input)
        {
            return new DirectoryDataSource(input.FullName);
        }
    }
}