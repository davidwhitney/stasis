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
                var siblingFiles = Directory.GetFiles(localItem.ContentRoot, "*.*", SearchOption.TopDirectoryOnly);

                var validTemplates = siblingFiles.Where(file => validTemplateFileNames.Any(file.Contains)).ToList();
                var supportedTemplates = validTemplates.Where(x => templateEngines.Any(engine => engine.Supports(x)));

                var firstSupportedTemplateFile = supportedTemplates.FirstOrDefault();
                if (firstSupportedTemplateFile == null)
                {
                    return new NoTemplate();
                }

                var engineForTemplate = templateEngines.First(x => x.Supports(firstSupportedTemplateFile));
                return engineForTemplate.CreateTemplateInstance(File.ReadAllBytes(firstSupportedTemplateFile));
            }

            return new NoTemplate();
        }
    }
}