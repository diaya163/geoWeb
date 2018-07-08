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
    /// mElementalAbundanceBLL
    /// </summary>
    public partial class mElementalAbundanceBLL : BaseBLL
    {
        private readonly mElementalAbundanceDAL dal = new mElementalAbundanceDAL();
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<mElementalAbundance> GetList(string filters, ref string errMsg)
        {
            return dal.GetList(filters, db, ref errMsg);
        }


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mElementalAbundance model, ref string errMsg)
        {
            return dal.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mElementalAbundance model, ref string errMsg)
        {
            return dal.Update(model, db, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mElementalAbundance model, ref string errMsg)
        {
            return dal.Delete(model, db, ref errMsg);
        }

        public List<mElementalAbundance> GetIndexList(string filters, ref string errMsg, ref GridPager pager)
        {

            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        private List<mElementalAbundance> CreateModelList(ref GridPager pager, ref IQueryable<mElementalAbundance> queryData)
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
                           select new mElementalAbundance
                           {
                               id = k.id,
                               Element = k.Element,
                               AbundanceVal = k.AbundanceVal,
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


