using System;
using System.Collections.Generic;
using System.IO;
using Stasis.DataSources;

namespace Stasis
{
    public class SiteConfiguration
    {
        public List<ContentRegistration> ContentRegistrations { get; } = new List<ContentRegistration>();

        public SiteConfiguration AddContent(string source, string outpath, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            if (source.StartsWith("~/") || source.StartsWith("./"))
            {
                source = source.Replace("~/", "");
                source = source.Replace("./", "");
                source = Path.Combine(Environment.CurrentDirectory, source);
            }

            if (!Directory.Exists(source))
            {
                throw new InvalidOperationException("Data source not recognised");
            }

            AddContent(new DirectoryDataSource(source), outpath, onContentRegistrationCreation);
            return this;
        }

        public SiteConfiguration AddContent(Func<IAsyncEnumerable<RawItem>> loadItemsFunction, string outpath, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            var delegatedDataSource = new DelegatedDataSource(loadItemsFunction);
            AddContent(delegatedDataSource, outpath, onContentRegistrationCreation);
            return this;
        }

        public SiteConfiguration AddContent(Func<IEnumerable<RawItem>> loadItemsFunction, string outpath, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            var delegatedDataSource = new DelegatedDataSource(loadItemsFunction);
            AddContent(delegatedDataSource, outpath, onContentRegistrationCreation);
            return this;
        }

        public SiteConfiguration AddContent(IDataSource source, string outpath, Action<ContentRegistration> onContentRegistrationCreation = null)
        {
            onContentRegistrationCreation ??= cr => { };

            var contentRegistration = new ContentRegistration
            {
                DataSource = source,
                OutputPath = outpath
            };

            onContentRegistrationCreation(contentRegistration);
            ContentRegistrations.Add(contentRegistration);
            return this;
        }
    }
}