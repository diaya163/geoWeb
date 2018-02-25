using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjGeo.Models;
using System.Data;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;

namespace prjGeo.BLL.Core
{
    public class BaseBLL : IDisposable
    {
        protected GeoGisEntities db = new GeoGisEntities();
        public void Dispose()
        {

        }
    }
}
