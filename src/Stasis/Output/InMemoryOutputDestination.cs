using System.Collections.Concurrent;
using System.IO;
using Stasis.TemplateEngines;

namespace Stasis.Output
{
    public class InMemoryOutputDestination : IOutputDestination
    {
        public ConcurrentDictionary<string, ProcessingResultBase> Files { get; set; } = new ConcurrentDictionary<string, ProcessingResultBase>();
        public void Save(string path, ProcessingResultBase contents) => Files[path] = contents;
        public void Copy(string path, string destination)
        {
            var bytes = File.ReadAllBytes(path);
            Save(destination, new BinaryResult(bytes, path));
        }
    }
}