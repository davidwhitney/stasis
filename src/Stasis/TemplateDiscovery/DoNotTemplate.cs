using Stasis.ContentModel;

namespace Stasis.TemplateDiscovery
{
    public class DoNotTemplate : IFindTemplates
    {
        public Template SelectTemplate() => new Template { Kind = "no-template" };
    }
}