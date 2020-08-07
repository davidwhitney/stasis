using System.Collections.Generic;
using Stasis.ContentModel;

namespace Stasis.TemplateEngines
{
    public interface ITemplateEngine
    {
        bool Supports(string templateName);
        ProcessingResultBase Process<TTemplateType>(Item item, TTemplateType template) where TTemplateType : ITemplate;
        ITemplate CreateTemplateInstance(byte[] templateBytes);
    }

    public interface ITemplateEngine<in TTemplateType> : ITemplateEngine
        where TTemplateType : ITemplate
    {
        ProcessingResultBase Process(Item item, TTemplateType template);
    }
}