namespace Stasis.TemplateEngines
{
    public abstract class ProcessingResultBase
    {
        public string OutputName { get; set; }
        public abstract byte[] ContentBytes { get; set; }

        protected ProcessingResultBase(string outputName)
        {
            OutputName = outputName;
        }
    }
}