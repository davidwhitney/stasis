using Stasis;
using System;
using Stasis.Output;

namespace BasicSiteExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var config = new SiteConfiguration()
                .AddContent("./Site", "/");

            var generator = new Generator();
            generator.Output = new FileSystemOutputDestination("c:\\dev\\outtest");

            generator.Generate(config).GetAwaiter().GetResult();
        }
    }
}
