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
    /// mElementDAL
    /// </summary>
    public partial class mElementDAL : IDisposable
    {

        public mElementDAL()
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
        public IList<mElement> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            mElement model = new mElement();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            IList<mElement> list = new List<mElement>();
            try
            {
                selCmd.AppendFormat("SELECT * FROM {0}  (Nolock)  ", "mElement");
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.Append(" ORDER BY id");
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                list = con.Database.SqlQuery<mElement>(selCmd.ToString()).ToList();

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
        public int Add(mElement model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder insCmd = new StringBuilder();

            insCmd.AppendFormat("insert into {0}", "mElement");
            insCmd.Append("(ElementSymbol,ElementName,UnitSymbol,ElementUnit,Decimals,Property1,Property2,Property3,Property4,Property5,Property6,Property7,Property8,Property9,Property10,Property11,Property12,Property13,Property14,Property15,Attachment,Remarks)");
            insCmd.Append("VALUES(@ElementSymbol,@ElementName,@UnitSymbol,@ElementUnit,@Decimals,@Property1,@Property2,@Property3,@Property4,@Property5,@Property6,@Property7,@Property8,@Property9,@Property10,@Property11,@Property12,@Property13,@Property14,@Property15,@Attachment,@Remarks)");
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
        public int UpdateAttachment(mElement model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mElement");
            updCmd.Append(" Attachment=@Attachment");
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
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="helper">数据库类</param>
        /// <param name="isTran">是否为事务</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mElement model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder updCmd = new StringBuilder();
            updCmd.AppendFormat("Update {0} Set ", "mElement");
            updCmd.Append("ElementSymbol=@ElementSymbol");
            updCmd.Append(",ElementName=@ElementName");
            updCmd.Append(",UnitSymbol=@UnitSymbol");
            updCmd.Append(",ElementUnit=@ElementUnit");
            updCmd.Append(",Decimals=@Decimals");
            updCmd.Append(",Property1=@Property1");
            updCmd.Append(",Property2=@Property2");
            updCmd.Append(",Property3=@Property3");
            updCmd.Append(",Property4=@Property4");
            updCmd.Append(",Property5=@Property5");
            updCmd.Append(",Property6=@Property6");
            updCmd.Append(",Property7=@Property7");
            updCmd.Append(",Property8=@Property8");
            updCmd.Append(",Property9=@Property9");
            updCmd.Append(",Property10=@Property10");
            updCmd.Append(",Property11=@Property11");
            updCmd.Append(",Property12=@Property12");
            updCmd.Append(",Property13=@Property13");
            updCmd.Append(",Property14=@Property14");
            updCmd.Append(",Property15=@Property15");
            updCmd.Append(",Attachment=@Attachment");
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
        public int Delete(mElement model, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            StringBuilder delCmd = new StringBuilder();
            delCmd.AppendFormat(" Delete {0} where ", "mElement");
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}


