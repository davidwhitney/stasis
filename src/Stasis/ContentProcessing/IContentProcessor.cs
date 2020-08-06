using System.Collections.Generic;
using Stasis.ContentModel;

namespace Stasis.ContentProcessing
{
    public interface IContentProcessor
    {
        bool Supports(Item item);
        ProcessingResultBase Process(Item item);
    }

    public abstract class ProcessingResultBase
    {
        public Dictionary<string, string> Properties { get; set; }
    }

    public class TextResult : ProcessingResultBase
    {
        public string Content { set; get; }

        public TextResult(string content, Dictionary<string, string> properties = null)
        {
            Content = content;
            Properties = properties ?? new Dictionary<string, string>();
        }
    }

    public class BinaryResult : ProcessingResultBase
    {
        public byte[] Content { get; set; }
    }
}