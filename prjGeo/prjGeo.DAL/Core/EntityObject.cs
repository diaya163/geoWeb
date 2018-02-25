using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;

namespace prjGeo.DAL.Core
{
    public class EntityObject : IDisposable
    {
        public ObjectContext GetObjectContext(DbContext db)
        {
            return ((IObjectContextAdapter)db).ObjectContext;
        }
        public DbContext GetDbContext(ObjectContext db)
        {
            return (DbContext)(db as IObjectContextAdapter);
        }

        public void Dispose()
        {

        }
    }
}
