using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stasis.ContentProcessing;
using Stasis.DataSources;
using Stasis.Output;

namespace Stasis
{
    public class SiteConfiguration
    {
        public List<ContentRegistration> ContentRegistrations { get; } = new List<ContentRegistration>();

        public SiteConfiguration AddContent(string source)
        {
            if (Directory.Exists(source))
            {
                AddContent(new DirectoryDataSource(source));
            }
            else
            {
                throw new InvalidOperationException("Data source not recognised");
            }

            return this;
        }

        public SiteConfiguration AddContent(IDataSource source)
        {
            var registration = new ContentRegistration
            {
                DataSource = source
            };

            ContentRegistrations.Add(registration);
            return this;
        }
    }

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
                    var bytes = processor.Process(item);

                    Output.Save(item.DestinationKey, bytes);
                }
            }
        }

    }
}
