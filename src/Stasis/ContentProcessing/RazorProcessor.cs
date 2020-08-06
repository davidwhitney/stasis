using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using Stasis.ContentModel;
using Encoding = System.Text.Encoding;

namespace Stasis.ContentProcessing
{
    // Razor files are kinda content, and kinda already templates as Razor is a templating engine
    // So if we're treating them as content, I guess we'll just return them fully formed.
    // The expectation is that if you're using razor, you'll be using razors default _Layouts
    // and we won't do any template resolution on top of the output of the Razor rendering

    public class RazorProcessor : IContentProcessor
    {
        public bool Supports(Item item)
        {
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);
            return fileDetails.Extension == ".cshtml";
        }

        public ProcessingResultBase Process(Item item)
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
            
            return new TextResult(result);
        }
    }
}