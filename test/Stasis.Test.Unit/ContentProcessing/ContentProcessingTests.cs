using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Stasis.ContentProcessing;
using Stasis.ContentProcessing.Markdown;

namespace Stasis.Test.Unit.ContentProcessing
{
    [TestFixture]
    public class MarkdownUnpackerTests
    {
        private MarkdownUnpacker _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MarkdownUnpacker();
        }

        [Test]
        public void Unpack_GivenMarkdownWithFrontMatter_ExtractsFrontMatter()
        {
            var markdown = @"
---
some: value
another: variable
---
# title";

            var result = _sut.Unpack(markdown.TrimStart());

            Assert.That(result.Body, Is.EqualTo("# title"));
            Assert.That(result.FrontMatter["some"], Is.EqualTo("value"));
            Assert.That(result.FrontMatter["another"], Is.EqualTo("variable"));
        }
    }
}
