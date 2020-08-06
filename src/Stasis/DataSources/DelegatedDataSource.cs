using System;
using System.Collections.Generic;

namespace Stasis.DataSources
{
    public class DelegatedDataSource : IDataSource
    {
        private readonly Func<IEnumerable<RawItem>> _getItemsFunction;
        private readonly Func<IAsyncEnumerable<RawItem>> _getItemsFunctionAsync;

        public DelegatedDataSource(Func<IEnumerable<RawItem>> getItemsFunction)
        {
            _getItemsFunction = getItemsFunction;
        }

        public DelegatedDataSource(Func<IAsyncEnumerable<RawItem>> getItemsFunction)
        {
            _getItemsFunctionAsync = getItemsFunction;
        }

        public async IAsyncEnumerable<RawItem> GetItems()
        {
            if (_getItemsFunction != null)
            {
                foreach (var item in _getItemsFunction())
                {
                    yield return item;
                }
            }
            else
            {
                var enumerable = _getItemsFunctionAsync();
                await foreach (var item in enumerable)
                {
                    yield return item;
                }

            }
        }
    }
}