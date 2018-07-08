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
    /// mGeologyUnitDAL
    /// </summary>
    public partial class mGeologyUnitDAL : IDisposable
    {

        public mGeologyUnitDAL()
        {

        }

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
        public IList<mGeologyUnit> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            mGeologyUnit model = new mGeologyUnit();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            IList<mGeologyUnit> list = new List<mGeologyUnit>();
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);

                selCmd.AppendFormat("SELECT * FROM {0}  (Nolock)  ", "mGeologyUnit");
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.Append(" ORDER BY id");

                list = con.Database.SqlQuery<mGeologyUnit>(selCmd.ToString()).ToList();

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
        public int Add(mGeologyUnit model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();
            insCmd.AppendFormat("insert into {0}", "mGeologyUnit");
            insCmd.Append("(GeoCode,GeoName,GeoNameCHN,GeoType,ProCode,Remarks)");
            insCmd.Append("VALUES(@GeoCode,@GeoName,@GeoNameCHN,@GeoType,@ProCode,@Remarks)");
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
                errMsg = ex.Message.Replace("'", string.Empty);
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
        public int Update(mGeologyUnit model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mGeologyUnit");
            updCmd.Append("GeoCode=@GeoCode");
            updCmd.Append(",GeoName=@GeoName");
            updCmd.Append(",GeoNameCHN=@GeoNameCHN");
            updCmd.Append(",GeoType=@GeoType");
            updCmd.Append(",ProCode=@ProCode");
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
                errMsg = ex.Message.Replace("'", string.Empty);
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
        public int Delete(mGeologyUnit model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", "mGeologyUnit");
            delCmd.Append("id=@id");

            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);

                return helper.ExecuteNonQuery(delCmd.ToString(), model);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message.Replace("'", string.Empty);
            }
            return -1;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}


