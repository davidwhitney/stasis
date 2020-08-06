using System.Collections.Concurrent;
using Stasis.TemplateEngines;

namespace Stasis.Output
{
    public class InMemoryOutputDestination : IOutputDestination
    {
        public ConcurrentDictionary<string, ProcessingResultBase> Files { get; set; } = new ConcurrentDictionary<string, ProcessingResultBase>();
        public void Save(string path, ProcessingResultBase contents) => Files[path] = contents;
    }
}