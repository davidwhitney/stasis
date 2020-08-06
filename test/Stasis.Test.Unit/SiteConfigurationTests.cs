using System;
using NUnit.Framework;
using Stasis.DataSources;
using Stasis.TemplateDiscovery;
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
            _sut.AddContent(TestContent.Location("SingleRazorFile"));

            Assert.That(_sut.ContentRegistrations.Count, Is.EqualTo(1));
            Assert.That(_sut.ContentRegistrations[0].DataSource, Is.TypeOf<DirectoryDataSource>());
        }

        [Test]
        public void AddDataSource_CanOverwriteTemplateSettingsUsingCallback()
        {
            _sut.AddContent(TestContent.Location("SingleRazorFile"), cr =>
            {
                cr.TemplateFinder = new MyRandomTemplateThing();
            });

            Assert.That(_sut.ContentRegistrations[0].TemplateFinder, Is.TypeOf<MyRandomTemplateThing>());
        }
        private class MyRandomTemplateThing : IFindTemplates
        {
        }
    }
}