using System;
using System.Collections.Generic;

namespace RDI.Web.Repository.DataContext
{
    public interface IDataContext<T> : IDisposable
    {
        IEnumerable<T> Get();
        T Get(object id);
        T Set(T item);
        IEnumerable<T> Set(List<T> items);
        T Delete(object id);
    }
}
