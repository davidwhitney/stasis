using System.Collections.Generic;

namespace Stasis.ContentModel
{
    public abstract class Item
    {
        public string SourceKey { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }

    public class TextItem : Item
    {
        public string Content { get; set; }

    }

    public class BinaryItem : Item
    {
        public byte[] Content { get; set; }
    }
}