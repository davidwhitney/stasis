using System;
using System.Collections.Generic;
using Stasis.ContentModel;

namespace Stasis.DataSources
{
    public class DelegatedDataSource : IDataSource
    {
        private readonly Func<IEnumerable<Item>> _getItemsFunction;
        private readonly Func<IAsyncEnumerable<Item>> _getItemsFunctionAsync;

        public DelegatedDataSource(Func<IEnumerable<Item>> getItemsFunction)
        {
            _getItemsFunction = getItemsFunction;
        }

        public DelegatedDataSource(Func<IAsyncEnumerable<Item>> getItemsFunction)
        {
            _getItemsFunctionAsync = getItemsFunction;
        }

        public async IAsyncEnumerable<Item> GetItems()
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