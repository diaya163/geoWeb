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
using prjGeo.Models.Buss;

using prjGeo.DAL;
using prjGeo.Commons;

namespace prjGeo.BLL
{
    public class HSAllSeqBLL : BaseBLL
    {
        private readonly HSAllSeqDAL dal = new HSAllSeqDAL();
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<HSAllSeq> GetList(string filters, ref string errMsg)
        {
            return dal.GetList(filters, db, ref errMsg);
        }
        public List<HSAllSeq> GetIndexList(HSAllSeq objFilterModel, ref string errMsg, ref GridPager pager)
        {
             
            string filters = string.Empty;
            if (objFilterModel != null)
            {
                if (objFilterModel.HSID != null || (objFilterModel.HSID + "").Trim() != "")
                {
                    filters = "  where HSID like '%" + objFilterModel.HSID + "%'";
                }
                if (objFilterModel.GeCata != null)
                {
                    // if (objFilterModel.GeCata.ToUpper().Equals("MSGCMB")) objFilterModel.GeCata = "";
                    if (!objFilterModel.GeCata.ToUpper().Equals("MSGCMB"))
                    {
                        if (filters.Length > 0)
                        {
                            filters = filters + " and GeCata='" + objFilterModel.GeCata + "'";
                        }
                        else
                        {
                            filters = "  where GeCata='" + objFilterModel.GeCata + "'";
                        }
                    }
                }

                if (objFilterModel.ValueCata != null)
                {
                    if (!objFilterModel.ValueCata.ToUpper().Equals("MSGCMB"))
                    {
                        if (filters.Length > 0)
                        {
                            filters = filters + " and ValueCata='" + objFilterModel.ValueCata + "'";
                        }
                        else
                        {
                            filters = "   where  ValueCata='" + objFilterModel.ValueCata + "'";
                        }
                    }
                }
            }
            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        public List<HSAllSeq> GetIndexList(string filters, ref string errMsg, ref GridPager pager)
        {
            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        private List<HSAllSeq> CreateModelList(ref GridPager pager, ref IQueryable<HSAllSeq> queryData)
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
                           select new HSAllSeq
                           {
                               id = k.id,
                               HSID=k.HSID,
                               HSResult=k.HSResult,
                               sumNAP=k.sumNAP,
                               SumSeqNO=k.SumSeqNO,
                               GeCata=k.GeCata,
                               ValueCata=k.ValueCata
                           }).ToList();
            return lstData;
        }

 

    }
}
