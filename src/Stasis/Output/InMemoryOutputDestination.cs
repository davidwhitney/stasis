using System.Collections.Concurrent;

namespace Stasis.Output
{
    public class InMemoryOutputDestination : IOutputDestination
    {
        public ConcurrentDictionary<string, byte[]> Files { get; set; } = new ConcurrentDictionary<string, byte[]>();
        public void Save(string path, byte[] contents) => Files[path] = contents;
    }
}