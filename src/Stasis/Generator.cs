using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stasis.ContentProcessing;
using Stasis.ContentProcessing.Markdown;
using Stasis.Output;

namespace Stasis
{
    public class Generator
    {
        public List<IContentProcessor> ContentProcessors { get; } = new List<IContentProcessor>();
        public IOutputDestination Output { get; set; } = new InMemoryOutputDestination();

        public Generator()
        {
            ContentProcessors.Add(new RazorProcessor());
            ContentProcessors.Add(new MarkdownProcessor());
        }

        public async Task Generate(SiteConfiguration config)
        {
            foreach (var registration in config.ContentRegistrations)
            {
                await foreach (var item in registration.DataSource.GetItems())
                {
                    var processor = ContentProcessors.First(x => x.Supports(item));
                    var contentProcessingResult = processor.Process(item);

                    Output.Save(item.DestinationKey, contentProcessingResult);
                }
            }
        }

    }
}
