using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using prjGeo.BLL.Core;
using prjGeo.Models;
using prjGeo.Models.Sys;
using prjGeo.DAL;
using prjGeo.Commons;
namespace prjGeo.BLL
{
    /// <summary>
    /// mZoneBLL
    /// </summary>
    public partial class mZoneBLL : BaseBLL
    {
        private readonly mZoneDAL dal = new mZoneDAL();
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<mZone> GetList(string filters, ref string errMsg)
        {
            return dal.GetList(filters, db, ref errMsg);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mZone model, ref string errMsg)
        {
            return dal.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mZone model, ref string errMsg)
        {
            return dal.Update(model, db, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mZone model, ref string errMsg)
        {
            return dal.Delete(model, db, ref errMsg);
        }

        public List<mZone> GetIndexList(string filters, ref string errMsg, ref GridPager pager)
        {
            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        private List<mZone> CreateModelList(ref GridPager pager, ref IQueryable<mZone> queryData)
        {
            pager.totalRows = queryData.Count();
            if (pager.totalRows > 0)
            {
                if (pager.page <= 1)
                {
                    queryData = queryData.Take(pager.rows);
                }
                else
                {
                    queryData = queryData.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                }
            }

            var lstData = (from k in queryData
                           select new mZone
                           {
                               id = k.id,
                               Recid = k.Recid,
                               RecName = k.RecName,
                               ProName = k.ProName,
                               Province = k.Province,
                               City = k.City,
                               County = k.County,
                               Town = k.Town,
                               Village = k.Village,
                               MapNumber = k.MapNumber,
                               SampleNumber = k.SampleNumber,
                               ProCode = k.ProCode,
                               CSYSX = k.CSYSX,
                               CSYSY = k.CSYSY,
                               CSYSType = k.CSYSType,
                               Flon = k.Flon,
                               Flat = k.Flat,
                               Cale = k.Cale,
                               SampleNo = k.SampleNo,
                               Custom1 = k.Custom1,
                               Custom2 = k.Custom2,
                               Custom3 = k.Custom3,
                               Custom4 = k.Custom4,
                               Custom5 = k.Custom5,
                               Remarks = k.Remarks
                           }).ToList();
            return lstData;

        }
   
    
    }
}


