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
    /// mProjectBLL
    /// </summary>
    public partial class mProjectBLL : BaseBLL
    {
        private readonly mProjectDAL dal = new mProjectDAL();
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<mProject> GetList(string filters, ref string errMsg)
        {
            return dal.GetList(filters, db, ref errMsg);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mProject model, ref string errMsg)
        {
            return dal.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mProject model, ref string errMsg)
        {
            return dal.Update(model, db, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mProject model, ref string errMsg)
        {
            return dal.Delete(model, db, ref errMsg);
        }

        public List<mProject> GetIndexList(string filters, ref string errMsg, ref GridPager pager)
        {
            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        private List<mProject> CreateModelList(ref GridPager pager, ref IQueryable<mProject> queryData)
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
                           select new mProject
                           {
                               id = k.id,
                               Ccode = k.Ccode,
                               Recid = k.Recid,
                               CName = k.CName,
                               RecName = k.RecName,
                               Clocation = k.Clocation,
                               Province = k.Province,
                               City = k.City,
                               County = k.County,
                               Town = k.Town,
                               Village = k.Village,
                           }).ToList();
            return lstData;

        }
    }
}


