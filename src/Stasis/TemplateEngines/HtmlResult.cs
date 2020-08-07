using System.Text;

namespace Stasis.TemplateEngines
{
    public class HtmlResult : ProcessingResultBase
    {
        public string Content { set; get; }

        public override byte[] ContentBytes
        {
            get => Encoding.UTF8.GetBytes(Content);
            set => Content = Encoding.UTF8.GetString(value);
        }

        public HtmlResult(string content, string outputPath) 
            : base(outputPath)
        {
            Content = content;
        }
    }
}