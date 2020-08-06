using System.Collections.Generic;
using Stasis.ContentModel;

namespace Stasis.DataSources
{
    public interface IDataSource
    {
        IAsyncEnumerable<RawItem> GetItems();
    }
}