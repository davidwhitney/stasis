using Stasis.DataSources;
using Stasis.TemplateDiscovery;

namespace Stasis
{
    public class ContentRegistration
    {
        public IDataSource DataSource { get; set; }
        public IFindTemplates TemplateFinder { get; set; } = new FindNearestTemplate();
    }
}