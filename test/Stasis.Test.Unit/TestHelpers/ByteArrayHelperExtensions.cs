using System.Text;

namespace Stasis.Test.Unit.TestHelpers
{
    public static class ByteArrayHelperExtensions
    {
        public static string AsString(this byte[] src) => Encoding.Default.GetString(src);

    }
}