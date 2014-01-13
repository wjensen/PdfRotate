using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public virtual T GetByID(object id)
        {
            return _context.GetbyId(id);
        }
    }
}
