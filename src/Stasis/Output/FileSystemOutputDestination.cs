using System.IO;
using Stasis.TemplateEngines;

namespace Stasis.Output
{
    public class FileSystemOutputDestination : IOutputDestination
    {
        public string DestinationPath { get; }

        public FileSystemOutputDestination(string destinationPath)
        {
            DestinationPath = destinationPath;
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
        }

        public void Save(string path, ProcessingResultBase contents)
        {
            path = path.TrimStart('/');
            var fullPath = Path.Combine(DestinationPath, path); 
            
            var dirOnly = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dirOnly))
            {
                Directory.CreateDirectory(dirOnly);
            }

            File.WriteAllBytes(fullPath, contents.ContentBytes);
        }

        public void Copy(string source, string destination)
        {
            destination = destination.TrimStart('/');
            destination = destination.Replace("/", "\\");
            var fullPath = Path.Combine(DestinationPath, destination);

            var dirOnly = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dirOnly))
            {
                Directory.CreateDirectory(dirOnly);
            }

            File.Copy(source, fullPath, true);
        }
    }
}