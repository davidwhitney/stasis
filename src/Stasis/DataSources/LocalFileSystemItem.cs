using System.IO;

namespace Stasis.DataSources
{
    public class LocalFileSystemItem : RawItem
    {
        public string ContentRoot { get; }
        public string ContentPath { get; set; }

        public LocalFileSystemItem(string contentRoot, string fullPath, byte[] content)
        {
            ContentRoot = contentRoot;
            ContentPath = fullPath;
            SourceKey = fullPath.Replace(contentRoot, "").Replace("\\", "/");

            var fileDetails = new FileInfo(fullPath);

            ContentType = fileDetails.Extension;
            Content = content;
        }
    }
}