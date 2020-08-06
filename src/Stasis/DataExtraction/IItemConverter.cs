using Stasis.ContentModel;

namespace Stasis.DataExtraction
{
    public interface IItemConverter
    {
        bool Supports(string fileExtension);
        Item ConvertToItem(byte[] fileContents);
    }
}