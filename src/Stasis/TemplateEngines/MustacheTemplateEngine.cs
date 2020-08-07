using System.Linq;
using System.Text;
using Stasis.ContentModel;
using Stubble.Core;
using Stubble.Core.Settings;

namespace Stasis.TemplateEngines
{
    public class MustacheTemplateEngine : ITemplateEngine<MustacheTemplate>
    {
        private readonly StubbleVisitorRenderer _compiler;

        public MustacheTemplateEngine()
        {
            var settings = new RendererSettingsBuilder()
                //.SetEncodingFunction(s => s) // No HTML encoding of content.
                .BuildSettings();

            _compiler = new StubbleVisitorRenderer(settings);
        }

        public bool Supports(string templateName)
        {
            return new [] {".mustache", ".mustache.html", ".mustache.txt", ".mustache.htm"}.Any(templateName.EndsWith);
        }

        public ProcessingResultBase Process<TTemplateType>(Item item, TTemplateType template) where TTemplateType : ITemplate
        {
            return Process(item, template as MustacheTemplate);
        }

        public ITemplate CreateTemplateInstance(byte[] templateBytes)
        {
            return new MustacheTemplate {Content = templateBytes};
        }

        public ProcessingResultBase Process(Item item, MustacheTemplate template)
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