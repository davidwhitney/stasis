using System;
using System.Collections.Generic;
using System.Linq;
using Stasis.ContentModel;

namespace Stasis.TemplateEngines
{
    public class NoTemplateEngine : ITemplateEngine
    {
        public List<string> SupportedExtensions { get; } = new List<string> {"no-template"};
        public ProcessingResultBase Process(Item item, Template template)
        {
            var extension = item.SourceKey.Split(".").Last();
            var htmlPath = item.SourceKey.Replace("." + extension, ".html");
            
            return item switch
            {
                TextItem textItem => new HtmlResult(textItem.Content, htmlPath),
                BinaryItem binaryItem => new BinaryResult(binaryItem.Content, item.SourceKey),
                _ => throw new InvalidOperationException("No Template Engine doesn't know what to do with this.")
            };
        }
    }
}