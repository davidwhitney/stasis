namespace Stasis.TemplateEngines
{
    public sealed class BinaryResult : ProcessingResultBase
    {
        public override byte[] ContentBytes { get; set; }

        public BinaryResult(byte[] contentBytes, string outputName)
            : base(outputName)
        {
            ContentBytes = contentBytes;
        }
    }
}