namespace Stasis.ContentModel.DataExtraction
{
    public interface IItemConverter
    {
        bool Supports(string fileExtension);
        Item ConvertToItem(byte[] fileContents);
    }
}