using System;
using NUnit.Framework;
using Stasis.DataSources;
using Stasis.Test.Unit.TestHelpers;

namespace Stasis.Test.Unit
{
    [TestFixture]
    public class SiteConfigurationTests
    {
        private SiteConfiguration _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SiteConfiguration();
        }

        [Test]
        public void AddDataSource_BadFolderPath_Throws()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => 
                _sut.AddContent(TestContent.Location("c:\\blah\\not\\exists")));

            Assert.That(ex.Message, Is.EqualTo("Data source not recognised"));
        }

        [Test]
        public void AddDataSource_ValidFolderPath_AddsSource()
        {
            var config = new SiteConfiguration().AddContent(TestContent.Location("SingleRazorFile"));

            Assert.That(config.ContentRegistrations.Count, Is.EqualTo(1));
            Assert.That(config.ContentRegistrations[0].DataSource, Is.TypeOf<DirectoryDataSource>());
        }
    }
}