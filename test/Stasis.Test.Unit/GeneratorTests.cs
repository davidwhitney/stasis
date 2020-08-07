using System.Threading.Tasks;
using NUnit.Framework;
using Stasis.Output;
using Stasis.Test.Unit.TestHelpers;

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
        public async Task Generate_SingleRazorFile_AddsHtmlFileToOutput()
        {
            var config = new SiteConfiguration().AddContent(TestContent.Location("SingleRazorFile"));

            await _sut.Generate(config);

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
        }

        [Test]
        public async Task Generate_SingleMarkdownFile_AddsHtmlFileToOutput()
        {
            var config = new SiteConfiguration().AddContent(TestContent.Location("SingleMarkdownFile"));

            await _sut.Generate(config);

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("<h1 id=\"this-is-some-text\">This is some text</h1>"));
        }

        [Test]
        public async Task Generate_SingleMarkdownFileWithLocalMustacheTemplate_TemplatesFile()
        {
            var config = new SiteConfiguration()
                .AddContent(TestContent.Location("SingleMarkdownFileWithSameDirTemplate"));

            await _sut.Generate(config);

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("<h1>This is from the template</h1>"));
            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("<h1 id=\"this-is-some-text\">This is some text</h1>"));
        }

        [Test]
        public async Task Generate_SingleMarkdownFileWithLocalMustacheTemplate_FrontMatterVariablesCanBeUsedInTemplate()
        {
            var config = new SiteConfiguration()
                .AddContent(TestContent.Location("SingleMarkdownFileWithSameDirTemplate"));

            await _sut.Generate(config);

            Assert.That(_output.Files["/Index.html"], Is.Not.Null);
            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("Expanded Variable"));
        }

        [Test]
        public async Task Generate_SingleRazorFile_EmbeddedCSharpExecuted()
        {
            var config = new SiteConfiguration().AddContent(TestContent.Location("SingleRazorFile"));

            await _sut.Generate(config);

            Assert.That(_output.Files["/Index.html"].AsString(), Does.Contain("Expanded Variable"));
        }
    }
}
