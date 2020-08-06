using System;
using System.Collections.Generic;
using System.IO;
using Stasis.ContentModel;
using Stasis.DataSources;
using Stasis.TemplateDiscovery;

namespace Stasis
{
    public class SiteConfiguration
    {
        public List<ContentRegistration> ContentRegistrations { get; } = new List<ContentRegistration>();

        public SiteConfiguration AddContent(string source, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            if (Directory.Exists(source))
            {
                AddContent(new DirectoryDataSource(source), onContentRegistrationCreation);
            }
            else
            {
                throw new InvalidOperationException("Data source not recognised");
            }

            return this;
        }

        public SiteConfiguration AddContent(Func<IAsyncEnumerable<RawItem>> loadItemsFunction, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            var delegatedDataSource = new DelegatedDataSource(loadItemsFunction);
            AddContent(delegatedDataSource, onContentRegistrationCreation);
            return this;
        }

        public SiteConfiguration AddContent(Func<IEnumerable<RawItem>> loadItemsFunction, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            var delegatedDataSource = new DelegatedDataSource(loadItemsFunction);
            AddContent(delegatedDataSource, onContentRegistrationCreation);
            return this;
        }

        public SiteConfiguration AddContent(IDataSource source, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            onContentRegistrationCreation ??= cr => { };

            var contentRegistration = new ContentRegistration
            {
                DataSource = source
            };

            onContentRegistrationCreation(contentRegistration);
            ContentRegistrations.Add(contentRegistration);
            return this;
        }
    }
}