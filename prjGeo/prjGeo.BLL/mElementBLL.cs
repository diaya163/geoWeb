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
    /// mElementBLL
    /// </summary>
    public partial class mElementBLL : BaseBLL
    {
        private readonly mElementDAL dal = new mElementDAL();
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="filters">查询条件</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public IList<mElement> GetList(string filters, ref string errMsg)
        {
            return dal.GetList(filters, db, ref errMsg);
        }


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mElement model, ref string errMsg)
        {
            model.Property1=model.Property1==null? "":model.Property1;
            model.Property2 = model.Property2 == null ? "" : model.Property2;
            model.Property3 = model.Property3 == null ? "" : model.Property3;
            model.Property4 = model.Property4 == null ? "" : model.Property4;
            model.Property5 = model.Property5 == null ? "" : model.Property5;
            model.Property6 = model.Property6 == null ? "" : model.Property6;
            model.Property7 = model.Property7 == null ? "" : model.Property7;
            model.Property8 = model.Property8 == null ? "" : model.Property8;
            model.Property9 = model.Property9 == null ? "" : model.Property9;
            model.Property10 = model.Property10 == null ? "" : model.Property10;
            model.Property11 = model.Property11 == null ? "" : model.Property11;
            model.Property12 = model.Property12 == null ? "" : model.Property12;
            model.Property13 = model.Property13 == null ? "" : model.Property13;
            model.Property14 = model.Property14 == null ? "" : model.Property14;
            model.Property15 = model.Property15 == null ? "" : model.Property15;
            model.Attachment = model.Attachment == null ? "" : model.Attachment;
            model.Remarks = model.Remarks == null ? "" : model.Remarks;

            return dal.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mElement model, ref string errMsg)
        {
            return dal.Update(model, db, ref errMsg);
        }
        public int UpdateAttachment(mElement model, ref string errMsg)
        {
            return dal.UpdateAttachment(model, db, ref errMsg);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mElement model, ref string errMsg)
        {
            return dal.Delete(model, db, ref errMsg);
        }

        public List<mElement> GetIndexList(string filters, ref string errMsg, ref GridPager pager)
        {
            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);
        }
        private List<mElement> CreateModelList(ref GridPager pager, ref IQueryable<mElement> queryData)
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
                           select new mElement
                           {
                               id = k.id,
                               ElementSymbol = k.ElementSymbol,
                               ElementName = k.ElementName,
                               UnitSymbol = k.UnitSymbol,
                               ElementUnit = k.ElementUnit,
                               Decimals = k.Decimals,
                               Property1 = k.Property1,
                               Property2 = k.Property2,
                               Property3 = k.Property3,
                               Property4 = k.Property4,
                               Property5 = k.Property5,
                               Property6 = k.Property6,
                               Property7 = k.Property7,
                               Property8 = k.Property8,
                               Property9 = k.Property9,
                               Property10 = k.Property10,
                               Property11 = k.Property11,
                               Property12 = k.Property12,
                               Property13 = k.Property13,
                               Property14 = k.Property14,
                               Property15 = k.Property15,
                               Attachment = k.Attachment,
                               Remarks = k.Remarks
                           }).ToList();
            return lstData;

        }
    }
}


