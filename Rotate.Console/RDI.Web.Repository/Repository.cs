using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDI.Web.Model;
using RDI.Web.Repository.DataContext;

namespace RDI.Web.Repository
{
    public class Repository<T> where T : class
    {
        internal IDataContext<T> _context;

        public Repository(IDataContext<T> context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> Get()
        {
            return _context.Get();
        }

        public virtual T Get(object id)
        {
            return _context.Get(id);
        }

        public virtual T Update(IModel item)
        {
            return _context.Set((T) item);
        }
        public virtual IEnumerable<T> Update(IEnumerable<T> items)
        {
            return _context.Set((List<T>) items);
        }
        public virtual T Delete(object id)
        {
            return _context.Delete(id);
        }

    }
}
