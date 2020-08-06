using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Templating;
using Stasis.ContentModel;
using Encoding = System.Text.Encoding;

namespace Stasis.TemplateEngines
{
    // Razor files are kinda content, and kinda already templates as Razor is a templating engine
    // So if we're treating them as content, I guess we'll just return them fully formed.
    // The expectation is that if you're using razor, you'll be using razors default _Layouts
    // and we won't do any template resolution on top of the output of the Razor rendering

    public class RazorTemplateEngine : ITemplateEngine
    {
        public List<string> SupportedExtensions { get; } = new List<string> {".cshtml"};

        public ProcessingResultBase Process(Item item, Template template)
        {
            // Load template file here, and use item as bound view model.

            var textItem = (TextItem)item;
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);

            var result = Engine
                .Razor
                .RunCompile(textItem.Content,
                    fileDetails.Name,
                    null,
                    new
                    {
                        Name = "World"
                    });

            var extension = item.SourceKey.Split(".").Last();
            var htmlPath = item.SourceKey.Replace("." + extension, ".html");

            return new HtmlResult(result, htmlPath);
        }
    }
}