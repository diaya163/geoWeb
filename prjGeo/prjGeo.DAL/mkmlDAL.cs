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
    public class mKmlDAL : IDisposable
    {
        public void Dispose()
        {
             
        }
        public mKmlDAL()
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
        public IList<mKml> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            mKml model = new mKml();
            if (string.IsNullOrEmpty(filters)) filters = " 1=1";
            StringBuilder selCmd = new StringBuilder();
            IList<mKml> list = new List<mKml>();
            try
            {
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                selCmd.Append("SELECT * FROM mKml (Nolock)  ");
                selCmd.AppendFormat(" where {0} ", filters);
                selCmd.Append(" ORDER BY layerOrder");
                list = con.Database.SqlQuery<mKml>(selCmd.ToString()).ToList();
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
        public int Add(mKml model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder insCmd = new StringBuilder();
            insCmd.Append("insert into mKml");
            insCmd.Append("(id,provinceName,cityName,countyName,longitude,latitude,isVisible,kmlPath,layerName,villageName,layerOrder,projId,prjName)");
            insCmd.Append("VALUES(@id,@provinceName,@cityName,@countyName,@longitude,@latitude,@isVisible,@kmlPath,@layerName,@villageName,@layerOrder,@projId,@prjName)");
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
        public int Update(mKml model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder updCmd = new StringBuilder();
            updCmd.Append("Update mKml Set ");
            updCmd.Append("provinceName=@provinceName");
            updCmd.Append(",cityName=@cityName");
            updCmd.Append(",countyName=@countyName");
            updCmd.Append(",villageName=@villageName");
            updCmd.Append(",isVisible=@isVisible");
            updCmd.Append(",layerName=@layerName");
            updCmd.Append(",layerOrder=@layerOrder");
            updCmd.Append(",projId=@projId");
            updCmd.Append(",prjName=@prjName");
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
        public int Delete(mKml model, GeoGisEntities db, ref string errMsg)
        {
            StringBuilder delCmd = new StringBuilder();
            delCmd.Append(" Delete mKml where ");
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
