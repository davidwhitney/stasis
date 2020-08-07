using System.Collections.Generic;
using Stasis.ContentModel;
using Stasis.DataSources;
using Stasis.TemplateEngines;

namespace Stasis.TemplateDiscovery
{
    public class DoNotTemplateStrategy : IFindTemplates
    {
        public ITemplate SelectTemplate(RawItem rawItem, List<ITemplateEngine> templateEngines) => new NullTemplate();
    }
}