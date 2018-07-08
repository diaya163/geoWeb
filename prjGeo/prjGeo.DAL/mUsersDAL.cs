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
    public class mUsersDAL:IDisposable
    {
        public mUsersDAL()
        { 
        }

        public IQueryable<mUser> GetAllList(GeoGisEntities db)
        {
            IQueryable<mUser> list = db.mUser.AsQueryable();
            return list;
        } 


        public List<mUsersModel> GetAllList(string strUser, GeoGisEntities db)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            List<mUsersModel> list = con.Database.SqlQuery<mUsersModel>("select * from mUser order by usercode").ToList();

            return list;
        }
        public List<mUsersModel> GetList(string strUser, string strUserCode, GeoGisEntities db)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            string strSQL = string.Format("select * from mUser where UserCode={0} order by OrderID", strUserCode);
            List<mUsersModel> list = con.Database.SqlQuery<mUsersModel>(strSQL).ToList();

            return list;
        }

        public mUser Login(string strUserCode, string strPwd)
        {
            try
            {
                using (GeoGisEntities db = new GeoGisEntities())
                {
                    mUser objUser = db.mUser.SingleOrDefault(c => c.UserCode == strUserCode.Trim() && c.Password == strPwd.Trim());
                    return objUser;
                }
            }
            catch (Exception ex)
            {
                 return null;
            }
        }

        public int UpdateUserLoginCountAndDate(string strUserCode,GeoGisEntities db)
        {            
            string strSQL = @"update muser set LoginCount = isnull(LoginCount,0) + 1,LastLoginDate = getdate() where UserCode = '{0}' ";
            return  (int)db.Database.ExecuteSqlCommand(strSQL, strUserCode);            
        }

        public void Dispose()
        {
             
        }


    }
}
