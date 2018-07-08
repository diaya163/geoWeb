using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjGeo.Models;
using prjGeo.Models.Sys;
using prjGeo.Models.Buss;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;
using Esquel.Utility;

namespace prjGeo.DAL
{
    public partial class HSAllSeqDAL : IDisposable
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

        public IList<HSAllSeq> GetList(string filters, GeoGisEntities db, ref string errMsg)
        {
            errMsg = string.Empty;
            HSAllSeq model = new HSAllSeq();
            if (string.IsNullOrEmpty(filters)) filters = "";
            StringBuilder selCmd = new StringBuilder();
            IList<HSAllSeq> list = new List<HSAllSeq>();
            try
            {
                
                selCmd.Append(@"SELECT a.* FROM U_HSAllSeq (Nolock)  a   "     );
                selCmd.AppendFormat(" {0} ", filters);
                selCmd.Append(" ORDER BY a.id");
                DbContext con = (DbContext)(db as IObjectContextAdapter);
                SQLHelper helper = new SQLHelper(con.Database.Connection.ConnectionString);
                list = helper.SelectReader<HSAllSeq>(selCmd.ToString());
                 
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return list;
        } 

        public void Dispose()
        {
        }

    }
}
