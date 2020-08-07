using System.Collections.Generic;
using Stasis.ContentModel;
using Stasis.DataSources;
using Stasis.TemplateEngines;

namespace Stasis.TemplateDiscovery
{
    public interface IFindTemplates
    {
        ITemplate SelectTemplate(RawItem rawItem, List<ITemplateEngine> templateEngines);
    }
}