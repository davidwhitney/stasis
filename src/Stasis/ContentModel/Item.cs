namespace Stasis.ContentModel
{
    public class Item
    {
        public string SourceKey { get; set; }
        public string DestinationKey { get; set; }
        public byte[] Content { get; set; }
    }
}