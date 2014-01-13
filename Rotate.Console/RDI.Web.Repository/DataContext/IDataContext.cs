using System;
using System.Collections.Generic;

namespace RDI.Web.Repository.DataContext
{
    public interface IDataContext<T> : IDisposable
    {
        IEnumerable<T> Get();
        T GetbyId(object id);
        T Set(T item);
        T Set(object id);
        T Delete(T item);
        T Delete(object id);
    }
}
