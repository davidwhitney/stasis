using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Stasis.Test.Unit
{
    [TestFixture]
    public class GeneratorTests
    {
        private Generator _sut;
        private InMemoryOutputDestination _output;

        [SetUp]
        public void SetUp()
        {
            _sut = new Generator();
            _output = new InMemoryOutputDestination();
            _sut.Output = _output;
        }

        [Test]
        public void AddDataSource_BadFolderPath_Throws()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _sut.AddDataSource(TestLocation("c:\\blah\\not\\exists")));

            Assert.That(ex.Message, Is.EqualTo("Data source not recognised"));
        }

        [Test]
        public void AddDataSource_ValidFolderPath_AddsSource()
        {
            _sut.AddDataSource(TestLocation("SingleRazorFile"));
            
            Assert.That(_sut.DataSources.Count, Is.EqualTo(1));
            Assert.That(_sut.DataSources[0], Is.TypeOf<DirectoryDataSource>());
        }

        [Test]
        public async Task Generate_SingleRazorFile_AddsHtmlFileToOutput()
        {
            await _sut.AddDataSource(TestLocation("SingleRazorFile"))
                      .Generate();

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
        }

        [Test]
        public async Task Generate_SingleMarkdownFile_AddsHtmlFileToOutput()
        {
            await _sut.AddDataSource(TestLocation("SingleMarkdownFile"))
                      .Generate();

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("<h1>This is some text</h1>"));

        }

        [Test]
        public async Task Generate_SingleRazorFile_EmbeddedCSharpExecuted()
        {
            await _sut.AddDataSource(TestLocation("SingleRazorFile"))
                      .Generate();

            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("Expanded Variable"));
        }

        private static string TestLocation(string partialPath)
        {
            // Do this properly
            return $"C:\\dev\\stasis\\test\\Stasis.Test.Unit\\Scenarios\\{partialPath}";
        }
    }

    public static class ByteArrayHelperExtensions
    {
        public static string AsString(this byte[] src) => Encoding.Default.GetString(src);
    }
}
