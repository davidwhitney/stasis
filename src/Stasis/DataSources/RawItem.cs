using System.IO;

namespace Stasis.DataSources
{
    public class RawItem
    {
        public string SourceKey { get; set; }
        public string ContentType { set; get; }
        public byte[] Content { get; set; }
    }
}