namespace Stasis.Test.Unit.TestHelpers
{
    public static class TestContent
    {
        public static string Location(string partialPath)
        {
            // Do this properly
            return $"C:\\dev\\stasis\\test\\Stasis.Test.Unit\\Scenarios\\{partialPath}";
        }
    }
}