using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stasis.DataExtraction;
using Stasis.DataSources;
using Stasis.Output;
using Stasis.TemplateEngines;

namespace Stasis
{
    public class Generator
    {
        public List<IItemConverter> ItemConverters { get; } = new List<IItemConverter>();
        public List<ITemplateEngine> TemplateEngines { get; } = new List<ITemplateEngine>();
        public IOutputDestination Output { get; set; } = new InMemoryOutputDestination();

        public Generator()
        {
            ItemConverters.Add(new RazorItemConverter());
            ItemConverters.Add(new MarkdownItemConverter());

            TemplateEngines.Add(new NoTemplateEngine());
            TemplateEngines.Add(new RazorTemplateEngine());
            TemplateEngines.Add(new MustacheTemplateEngine());
        }

        public async Task Generate(SiteConfiguration config)
        {
            foreach (var registration in config.ContentRegistrations)
            {
                await foreach (var rawItem in registration.DataSource.GetItems())
                {
                    ProcessContentItem(rawItem, registration);
                }
            }
        }

        private void ProcessContentItem(RawItem rawItem, ContentRegistration registration)
        {
            var template = registration.TemplateFinder.SelectTemplate();
            var processor = TemplateEngines.First(x => x.SupportedExtensions.Contains(template.Kind));
            var converter = ItemConverters.First(x => x.Supports(rawItem.ContentType));

            var item = converter.ConvertToItem(rawItem.Content);
            item.SourceKey = rawItem.SourceKey;
            var contentProcessingResult = processor.Process(item, template);

            Output.Save(contentProcessingResult.OutputPath, contentProcessingResult);
        }
    }
}
