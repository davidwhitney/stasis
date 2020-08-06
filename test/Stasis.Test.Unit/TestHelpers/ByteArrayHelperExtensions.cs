using System.Text;
using Stasis.ContentProcessing;

namespace Stasis.Test.Unit.TestHelpers
{
    public static class ByteArrayHelperExtensions
    {
        public static string AsString(this ProcessingResultBase src)
        {
            return src is TextResult result ? result.Content : null;
        }
    }
}