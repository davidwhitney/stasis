using System;
using System.Linq;
using Stasis.ContentModel;

namespace Stasis.TemplateEngines
{
    public class NoTemplateEngine : ITemplateEngine<NullTemplate>
    {
        public ProcessingResultBase Process<TTemplateType>(Item item, TTemplateType template) where TTemplateType : ITemplate
        {
            return Process(item, template as NullTemplate);
        }

        public bool Supports(string templateName) => false;

        public ITemplate CreateTemplateInstance(byte[] templateBytes)
        {
            return new NoTemplate();
        }

        public ProcessingResultBase Process(Item item, NullTemplate template)
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