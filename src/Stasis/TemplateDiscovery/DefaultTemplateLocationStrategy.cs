using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stasis.ContentModel;
using Stasis.DataSources;
using Stasis.TemplateEngines;

namespace Stasis.TemplateDiscovery
{
    public class DefaultTemplateLocationStrategy : IFindTemplates
    {
        public ITemplate SelectTemplate(RawItem rawItem, List<ITemplateEngine> templateEngines)
        {
            var validTemplateFileNames = new []
            {
                "_Layout",
                "_Template",
            };

            // collect all template engines supported
            // walk up directory tree following conventions looking for templates matching those extensions in order of occurence

            if (rawItem is LocalFileSystemItem localItem)
            {
                var rootLocation = localItem.ContentRoot;
                var siblingFiles = Directory.GetFiles(rootLocation, "*.*", SearchOption.TopDirectoryOnly);

                foreach (var file in siblingFiles)
                {
                    if (!validTemplateFileNames.Any(templatePrefix => file.Contains(templatePrefix)))
                    {
                        continue;
                    }

                    var supportingEngine = templateEngines.FirstOrDefault(x => x.Supports(file));

                    if (supportingEngine != null)
                    {
                        // Got a supported template!
                        return supportingEngine.CreateTemplateInstance(File.ReadAllBytes(file));
                    }
                }
            }

            return new NoTemplate();
        }
    }
}