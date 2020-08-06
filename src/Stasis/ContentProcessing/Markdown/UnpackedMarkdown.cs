using System.Collections.Generic;

namespace Stasis.ContentProcessing.Markdown
{
    public class UnpackedMarkdown
    {
        public Dictionary<string, string> FrontMatter { get; set; } = new Dictionary<string, string>();
        public string Body { get; set; } = "";
    }
}