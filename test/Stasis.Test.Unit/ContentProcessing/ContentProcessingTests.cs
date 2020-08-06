using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Stasis.ContentModel.DataExtraction;

namespace Stasis.Test.Unit.ContentProcessing
{
    [TestFixture]
    public class MarkdownItemConverterTests
    {
        private MarkdownItemConverter _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MarkdownItemConverter();
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

            var result = _sut.Extract(markdown.TrimStart());

            Assert.That(result.Content, Is.EqualTo("# title"));
            Assert.That(result.Properties["some"], Is.EqualTo("value"));
            Assert.That(result.Properties["another"], Is.EqualTo("variable"));
        }
    }
}
