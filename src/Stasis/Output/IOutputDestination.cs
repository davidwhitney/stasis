namespace Stasis.Output
{
    public interface IOutputDestination
    {
        void Save(string path, byte[] contents);
    }
}