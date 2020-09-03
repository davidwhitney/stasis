using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stasis.ContentModel;
using Stasis.DataExtraction;
using Stasis.DataSources;
using Stasis.Output;
using Stasis.TemplateDiscovery;
using Stasis.TemplateEngines;

namespace Stasis
{
    public class Generator
    {
        public List<IItemConverter> ItemConverters { get; } = new List<IItemConverter>();
        public List<ITemplateEngine> SupportedTemplateEngines { get; } = new List<ITemplateEngine>();
        public IOutputDestination Output { get; set; } = new InMemoryOutputDestination();

        public Generator()
        {
            ItemConverters.Add(new RazorItemConverter());
            ItemConverters.Add(new MarkdownItemConverter());

            SupportedTemplateEngines.Add(new NoTemplateEngine());
            SupportedTemplateEngines.Add(new RazorTemplateEngine());
            SupportedTemplateEngines.Add(new MustacheTemplateEngine());
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
            var converter = ItemConverters.FirstOrDefault(x => x.Supports(rawItem.ContentType));
            if (converter == null)
            {
                CopyUnconvertableFile(rawItem, registration);
                return;
            }

            var template = registration.DataSource is IFindTemplates templateSourcing
                ? templateSourcing.SelectTemplate(rawItem, SupportedTemplateEngines)
                : registration.TemplateFinder.SelectTemplate(rawItem, SupportedTemplateEngines);

            var templateEngine = SupportedTemplateEngines.TemplateEngineForTemplate(template);

            var item = converter.ConvertToItem(rawItem.Content);
            item.SourceKey = rawItem.SourceKey;
            var contentProcessingResult = templateEngine.Process(item, template);

            var outputPath = ProcessPath(contentProcessingResult.OutputName);
            var fullOutputPath = Path.Combine(registration.OutputPath, outputPath);

            Output.Save(fullOutputPath, contentProcessingResult);
        }

        private void CopyUnconvertableFile(RawItem rawItem, ContentRegistration registration)
        {
            var copyOuputPath = Path.Combine(registration.OutputPath, rawItem.SourceKey);

            switch (rawItem)
            {
                case LocalFileSystemItem localFile:
                    Output.Copy(localFile.ContentPath, copyOuputPath);
                    break;
                default:
                    Output.Save(copyOuputPath, new BinaryResult(rawItem.Content, rawItem.SourceKey));
                    break;
            }
        }

        private string ProcessPath(string outputPath)
        {
            if (outputPath.ToLower().EndsWith("index.html"))
            {
                return outputPath;
            }

            var extension = Path.GetExtension(outputPath);
            return outputPath.Replace(extension, "/index.html");
        }
    }

    public static class TypeExtensions
    {
        public static ITemplateEngine TemplateEngineForTemplate(this IEnumerable<ITemplateEngine> engines, ITemplate template)
        {
            return engines.FirstOrDefault(te => te.GetType().IsForTemplate(template))
                   ?? new NoTemplateEngine();
        }

        public static bool IsForTemplate(this Type type, ITemplate template)
        {
            var templateEngineGenericInterfaceTypeArg = type.GetInterfaces()
                .Single(i => i.Name == "ITemplateEngine`1")
                .GetGenericArguments()
                .Single();

            return templateEngineGenericInterfaceTypeArg == template.GetType();
        }
    }
}
