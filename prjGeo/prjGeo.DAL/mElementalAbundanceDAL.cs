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
    /// mElementalAbundanceDAL
    /// </summary>
    public partial class mElementalAbundanceDAL : IDisposable
    {
        public void Dispose()
        {
        }
        public mElementalAbundanceDAL()
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
        public IList<mElementalAbundance> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            mElementalAbundance model = new mElementalAbundance();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            IList<mElementalAbundance> list = new List<mElementalAbundance>();

            selCmd.Append("SELECT * FROM mElementalAbundance (Nolock)  ");
            selCmd.AppendFormat(" where {0} ", filters);
            selCmd.Append(" ORDER BY id");
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                list = con.Database.SqlQuery<mElementalAbundance>(selCmd.ToString()).ToList();
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
        public int Add(mElementalAbundance model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder insCmd = new StringBuilder();
            insCmd.AppendFormat("insert into {0}", "mElementalAbundance");
            insCmd.Append("(Element,AbundanceVal,Custom1,Custom2,Custom3,Custom4,Custom5,Remarks)");
            insCmd.Append("VALUES(@Element,@AbundanceVal,@Custom1,@Custom2,@Custom3,@Custom4,@Custom5,@Remarks)");
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
        public int Update(mElementalAbundance model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mElementalAbundance");
            updCmd.Append("Element=@Element");
            updCmd.Append(",AbundanceVal=@AbundanceVal");
            updCmd.Append(",Custom1=@Custom1");
            updCmd.Append(",Custom2=@Custom2");
            updCmd.Append(",Custom3=@Custom3");
            updCmd.Append(",Custom4=@Custom4");
            updCmd.Append(",Custom5=@Custom5");
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
        public int Delete(mElementalAbundance model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", "mElementalAbundance");
            delCmd.Append("id=@id");

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
    }
}


