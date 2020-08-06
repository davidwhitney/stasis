using System;
using System.Text;

namespace Stasis.ContentModel.DataExtraction
{
    public class MarkdownItemConverter : IItemConverter
    {
        public bool Supports(string fileExtension) => fileExtension == ".md";

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

            var result = new TextItem
            {
                Content = parts[1].TrimStart()
            };

            var variables = parts[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var variable in variables)
            {
                var variableParts = variable.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var key = variableParts[0].Trim();
                var value = variableParts[1].Trim();
                result.Properties[key] = value;
            }

            return result;
        }
    }
}
