using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stasis.ContentModel;
using Stubble.Core;
using Stubble.Core.Settings;

namespace Stasis.TemplateEngines
{
    public class MustacheTemplateEngine : ITemplateEngine
    {
        private StubbleVisitorRenderer _compiler;
        public List<string> SupportedExtensions { get; } = new List<string> {".html", ".htm", ".txt"};

        public MustacheTemplateEngine()
        {
            var settings = new RendererSettingsBuilder()
                .SetEncodingFunction(s => s) // No HTML encoding of content.
                .BuildSettings();

            _compiler = new StubbleVisitorRenderer(settings);
        }

        public ProcessingResultBase Process(Item item, Template template)
        {
            var textItem = (TextItem) item;
            var templateString = Encoding.UTF8.GetString(template.Content);

            var renderedTemplate = _compiler.Render(templateString, textItem);

            var extension = item.SourceKey.Split(".").Last();
            var htmlPath = item.SourceKey.Replace("." + extension, ".html");

            return new HtmlResult(renderedTemplate, htmlPath);
        }
    }
}