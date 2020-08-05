using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using Stasis.ContentModel;
using Encoding = System.Text.Encoding;

namespace Stasis.ContentProcessing
{
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
}