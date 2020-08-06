using System;

namespace Stasis.ContentProcessing.Markdown
{
    public class MarkdownUnpacker
    {
        public UnpackedMarkdown Unpack(string fileContents)
        {
            if (!fileContents.StartsWith("---"))
            {
                throw new NotImplementedException();
            }

            var skipFirstThreeDashes = fileContents.TrimStart((char) 65279, '-', '\r', '\n');
            var parts = skipFirstThreeDashes.Split("---", StringSplitOptions.RemoveEmptyEntries);
                
            var result = new UnpackedMarkdown
            {
                Body = parts[1].TrimStart()
            };

            var variables = parts[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var variable in variables)
            {
                var variableParts = variable.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var key = variableParts[0].Trim();
                var value = variableParts[1].Trim();
                result.FrontMatter[key] = value;
            }

            return result;
        }
    }
}