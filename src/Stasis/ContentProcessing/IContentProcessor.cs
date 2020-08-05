using Stasis.ContentModel;

namespace Stasis.ContentProcessing
{
    public interface IContentProcessor
    {
        bool Supports(Item item);
        byte[] Process(Item item);
    }
}