namespace Stasis.TemplateEngines
{
    public abstract class ProcessingResultBase
    {
        public string OutputPath { get; set; }
        public abstract byte[] ContentBytes { get; set; }

        protected ProcessingResultBase(string outputPath)
        {
            OutputPath = outputPath;
        }
    }
}