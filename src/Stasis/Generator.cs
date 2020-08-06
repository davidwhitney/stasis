using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stasis.ContentModel.DataExtraction;
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

            TemplateEngines.Add(new RazorTemplateEngine());
            TemplateEngines.Add(new MarkdownTemplateEngine());
        }

        public async Task Generate(SiteConfiguration config)
        {
            foreach (var registration in config.ContentRegistrations)
            {
                await foreach (var rawItem in registration.DataSource.GetItems())
                {
                    var converter = ItemConverters.First(x => x.Supports(rawItem.ContentType));
                    var item = converter.ConvertToItem(rawItem.Content);
                    item.SourceKey = rawItem.SourceKey;

                    // locate template using the templating strategy
                    // apply template to the content
                    var template = new Template
                    {
                        Kind = ".md",
                        Content = Encoding.UTF8.GetBytes("{{Content}}")
                    };

                    var processor = TemplateEngines.First(x => x.SupportedExtensions.Contains(template.Kind));
                    var contentProcessingResult = processor.Process(item, template);

                    Output.Save(contentProcessingResult.OutputPath, contentProcessingResult);
                }
            }
        }
    }

    public class Template
    {
        public string Kind { get; set; }
        public byte[] Content { get; set; }
    }
}
