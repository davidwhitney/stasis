using Stasis.ContentProcessing;

namespace Stasis.Output
{
    public interface IOutputDestination
    {
        void Save(string path, ProcessingResultBase contents);
    }
}