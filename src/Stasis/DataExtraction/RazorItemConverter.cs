using System;
using RazorEngine;
using RazorEngine.Templating;
using Stasis.ContentModel;
using Encoding = System.Text.Encoding;

namespace Stasis.DataExtraction
{
    public class RazorItemConverter : IItemConverter
    {
        public bool Supports(string fileExtension) => fileExtension == ".cshtml";

        public Item ConvertToItem(byte[] fileContents)
        {
            var templateText = Encoding.UTF8.GetString(fileContents);

            var result = Engine
                .Razor
                .RunCompile(templateText,
                    Guid.NewGuid().ToString(),
                    null,
                    new
                    {
                        Name = "World"
                    });

            return new TextItem
            {
                Content = result,
                RawContent = templateText
            };
        }
    }
}
