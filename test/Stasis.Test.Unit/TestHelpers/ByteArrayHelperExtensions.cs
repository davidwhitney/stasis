using Stasis.TemplateEngines;

namespace Stasis.Test.Unit.TestHelpers
{
    public static class ByteArrayHelperExtensions
    {
        public static string AsString(this ProcessingResultBase src)
        {
            return src is HtmlResult result ? result.Content : null;
        }
    }
}