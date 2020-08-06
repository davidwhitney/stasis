using System;
using System.Collections.Generic;
using System.Text;
using Markdig;
using Stasis.ContentModel;

namespace Stasis.DataExtraction
{
    public class MarkdownItemConverter : IItemConverter
    {
        private readonly MarkdownPipeline _pipeline;
        public bool Supports(string fileExtension) => fileExtension == ".md";
                
        public MarkdownItemConverter()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public Item ConvertToItem(byte[] fileContents) 
            => Extract(Encoding.UTF8.GetString(fileContents));
        
        public TextItem Extract(string fileContents)
        {
            if (!fileContents.StartsWith("---"))
            {
                return new TextItem
                {
                    Content = fileContents
                };
            }

            var skipFirstThreeDashes = fileContents.TrimStart((char)65279, '-', '\r', '\n');
            var parts = skipFirstThreeDashes.Split("---", StringSplitOptions.RemoveEmptyEntries);

            var rawContent = parts[1].TrimStart();
            var contentAsHtml = Markdown.ToHtml(rawContent, _pipeline);
            
            return new TextItem
            {
                RawContent = rawContent,
                Content = contentAsHtml, 
                Properties = ParseFrontMatter(parts)
            };
        }

        private static Dictionary<string, string> ParseFrontMatter(string[] parts)
        {
            var properties = new Dictionary<string, string>();
            var variables = parts[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var variable in variables)
            {
                var variableParts = variable.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var key = variableParts[0].Trim();
                var value = variableParts[1].Trim();
                properties[key] = value;
            }

            return properties;
        }
    }
}
