using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjGeo.Models;
using prjGeo.Models.Sys;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;

namespace prjGeo.DAL
{
    public class menuDAL : IDisposable
    {
        public IQueryable<mMenu> GetList(GeoGisEntities db)
        {
            IQueryable<mMenu> list = db.mMenu.AsQueryable();
            return list;
        }
         
        public IEnumerable<mMenu> GetListforSQL(GeoGisEntities db)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            IEnumerable<mMenu> list = con.Database.SqlQuery<mMenu>("select * from mMenu where IsVisible=1 order by id").ToList();

            return list;
        }
        public IEnumerable<mMenu> GetMenuByPersonId(string strUser, string id, GeoGisEntities db)
        {
            string strSQL = string.Empty;
            if (id == "0")
            {
                strSQL = "select * from mMenu  where  ParentID='" + id.Trim() + "'  and IsVisible=1 order by MenuID ";
            }
            else
            {
                strSQL = string.Format("select * from mMenu  where MenuID<>'{0}' and ParentID = '{0}' and IsVisible=1 order by MenuID ", id.Trim());
            }
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            IEnumerable<mMenu> list = con.Database.SqlQuery<mMenu>(strSQL).ToList();

            return list;
        }
        public List<mMenu> GetList(string strUser, string id, GeoGisEntities db)
        {
            string strSQL = string.Empty;
            if (id == "0")
            {
                strSQL = "select * from mMenu  where  ParentID='" + id.Trim() + "' and IsVisible=1 order by MenuID ";
            }
            else
            {
                strSQL = string.Format("select * from mMenu  where MenuID<>'{0}' and ParentID = '{0}' and IsVisible=1 order by MenuID ", id.Trim());
            }
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            List<mMenu> list = con.Database.SqlQuery<mMenu>(strSQL).ToList();

            return list;
        }

        public void Dispose()
        {

        }
    }
}
