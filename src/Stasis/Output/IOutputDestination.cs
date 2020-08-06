using Stasis.TemplateEngines;

namespace Stasis.Output
{
    public interface IOutputDestination
    {
        void Save(string path, ProcessingResultBase contents);
    }
}