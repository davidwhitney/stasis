using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig;
using Stasis.ContentModel;

namespace Stasis.TemplateEngines
{
    public class MarkdownTemplateEngine : ITemplateEngine
    {
        private readonly MarkdownPipeline _pipeline;

        public MarkdownTemplateEngine()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public List<string> SupportedExtensions { get; } = new List<string> {".md"}; 

        public ProcessingResultBase Process(Item item, Template template)
        {
            var textItem = (TextItem)item;
            var asString = Markdown.ToHtml(textItem.Content, _pipeline);
            var templateString = Markdown.ToHtml(textItem.Content, _pipeline);

            var materialised = templateString.Replace("{{Content}}", asString);

            var extension = item.SourceKey.Split(".").Last();
            var htmlPath = item.SourceKey.Replace("." + extension, ".html");

            return new TextResult(materialised)
            {
                OutputPath = htmlPath
            };
        }
    }
}