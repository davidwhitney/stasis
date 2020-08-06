using System.IO;
using System.Text;
using Markdig;
using Stasis.ContentModel;

namespace Stasis.ContentProcessing.Markdown
{
    public class MarkdownProcessor : IContentProcessor
    {
        private readonly MarkdownPipeline _pipeline;
        private readonly MarkdownUnpacker _unpacker;

        public MarkdownProcessor()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _unpacker = new MarkdownUnpacker();
        }

        public bool Supports(Item item)
        {
            var filePath = item.SourceKey;
            var fileDetails = new FileInfo(filePath);
            return fileDetails.Extension == ".md";
        }

        public ProcessingResultBase Process(Item item)
        {
            var content = Encoding.Default.GetString(item.Content);
            var unpacked = _unpacker.Unpack(content);
            var asString = Markdig.Markdown.ToHtml(unpacked.Body, _pipeline);

            return new TextResult(asString, unpacked.FrontMatter);
        }
    }
}