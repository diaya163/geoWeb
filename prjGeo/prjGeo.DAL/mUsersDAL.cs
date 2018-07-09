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
using Esquel.Utility;


namespace prjGeo.DAL
{
    public class mUsersDAL:IDisposable
    {
        public mUsersDAL()
        { 
        }

        //public IQueryable<mUser> GetAllList(GeoGisEntities db)
        //{
        //    IQueryable<mUser> list = db.mUser.AsQueryable();
        //    return list;
        //}

        public IList<mUsersModel> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            mUsersModel model = new mUsersModel();
            if (string.IsNullOrEmpty(filters)) filters = "";
            StringBuilder selCmd = new StringBuilder();
            IList<mUsersModel> list = new List<mUsersModel>();
            try
            {

                selCmd.Append(@"SELECT a.* FROM mUser (Nolock)  a   ");
                selCmd.AppendFormat(" {0} ", filters);
                selCmd.Append(" ORDER BY a.id");
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);
                list = helper.SelectReader<mUsersModel>(selCmd.ToString());

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
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
        #region 用户添加，删除，修改的操作
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>

        public int Add(mUser model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();
            insCmd.AppendFormat("insert into {0}", "mUser");
            insCmd.Append("(UserCode,DeptName,UserSeq,UserName,Description,Password,OrganizeName,ConfigJSON,IsEnable,LastLoginDate,CreatePerson,UpdatePerson)");
            insCmd.Append("VALUES(@UserCode,@DeptName,@UserSeq,@UserName,@Description,@Password,@OrganizeName,@ConfigJSON,@IsEnable,@LastLoginDate,@CreatePerson,@UpdatePerson)");
            insCmd.Append(";select @@IDENTITY");
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);
                object obj = helper.ExecuteScalar(insCmd.ToString(), model);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return -1;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mUser model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mUser");
            updCmd.Append("UserCode     =@UserCode      ");
            updCmd.Append(",DeptName     =@DeptName      ");
            updCmd.Append(",UserSeq      =@UserSeq       ");
            updCmd.Append(",UserName     =@UserName      ");
            updCmd.Append(",Description  =@Description   ");
            updCmd.Append(",Password     =@Password      ");
           // updCmd.Append(",RoleName     =@RoleName      ");
            updCmd.Append(",OrganizeName =@OrganizeName  ");
            updCmd.Append(",ConfigJSON   =@ConfigJSON    ");
            updCmd.Append(",IsEnable     =@IsEnable      ");
            //updCmd.Append(",LoginCount   =@LoginCount    ");
            updCmd.Append(",LastLoginDate=@LastLoginDate ");
            updCmd.Append(",CreatePerson =@CreatePerson  ");
            updCmd.Append(",CreateDate   =@CreateDate    ");
            updCmd.Append(",UpdatePerson =@UpdatePerson  ");
            updCmd.Append(",UpdateDate   =@UpdateDate    ");

            updCmd.Append(" where id=@id");
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);

                return helper.ExecuteNonQuery(updCmd.ToString(), model);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return -1;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mUser model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", "mUser");
            delCmd.AppendFormat("id={0}", model.id);
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);

                return helper.ExecuteNonQuery(delCmd.ToString(), model);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return -1;
        }
        #endregion
        
        public mUser Login(string strUserCode, string strPwd)
        {
            try
            {
                using (GeoGisEntities db = new GeoGisEntities())
                {
                    mUser objUser = db.mUser.SingleOrDefault(c => c.UserCode == strUserCode.Trim() && c.Password == strPwd.Trim() && c.IsEnable==true);
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
