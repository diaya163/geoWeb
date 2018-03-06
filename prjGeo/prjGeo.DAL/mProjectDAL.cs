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
    /// <summary>
    /// mProjectDAL
    /// </summary>
    public partial class mProjectDAL : IDisposable
    {

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="orderBy">字段排序</param>
        /// <param name="TopNo">前几行</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="rCount">总页数</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<mProject> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            mProject model = new mProject();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            IList<mProject> list = new List<mProject>();
            try
            {
                selCmd.Append("SELECT a.*,b.Memo as RecName FROM mProject (Nolock) a left join mArea (nolock) b on  b.id = a.Recid");
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.Append(" ORDER BY a.id");

                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);
                list = helper.SelectReader<mProject>(selCmd.ToString());

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return list;
        }


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mProject model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();
            insCmd.Append("Insert mProject(Recid,Ccode,CName,Clocation,Province,City,County,Town,Village,Remarks)");
            insCmd.Append("VALUES(@Recid,@Ccode,@CName,@Clocation,@Province,@City,@County,@Town,@Village,@Remarks)");
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
        public int Update(mProject model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mProject");
            updCmd.Append("Recid=@Recid");
            updCmd.Append(",Ccode=@Ccode");
            updCmd.Append(",CName=@CName");
            updCmd.Append(",Clocation=@Clocation");
            updCmd.Append(",Province=@Province");
            updCmd.Append(",City=@City");
            updCmd.Append(",County=@County");
            updCmd.Append(",Town=@Town");
            updCmd.Append(",Village=@Village");
            updCmd.Append(",Remarks=@Remarks");

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
        public int Delete(mProject model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", "mProject");
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

        public void Dispose()
        {
        }
    }
}


