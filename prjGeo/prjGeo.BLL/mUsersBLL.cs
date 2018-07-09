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
    public class mUsersBLL : BaseBLL
    {
        //mUsersDAL objUser = new mUsersDAL();
        private mUsersDAL objUser = new mUsersDAL();
        public mUsersBLL() { }
        public List<mUsersModel> GetAllList()
        {
            try
            {
                List<mUsersModel> queryData = null;
                queryData = objUser.GetAllList("", db);

                return queryData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IList<mUsersModel> GetList(string filters, ref string errMsg)
        {
            return objUser.GetList(filters, db, ref errMsg);
        }
        public List<mUsersModel> GetIndexList(mUsersModel objFilterModel, ref string errMsg, ref GridPager pager)
        {
            if (objFilterModel == null) return null;
            string filters = string.Empty;
            if (objFilterModel.UserCode != null || (objFilterModel.UserCode + "").Trim() != "")
            {
                filters = "  where UserCode = '" + objFilterModel.UserCode + "'";
            }
            if (objFilterModel.UserName != null || (objFilterModel.UserName + "").Trim() != "")
            {
                if (filters.Length > 0)
                {
                    filters = filters + " and UserName like '%" + objFilterModel.UserName + "%'";
                }
                else
                {
                    filters = "  where UserName like '%" + objFilterModel.UserName + "%'";
                }
            }

            if (objFilterModel.DeptName != null || (objFilterModel.DeptName + "").Trim() != "")
            {
                if (filters.Length > 0)
                {
                    filters = filters + " and DeptName like '%" + objFilterModel.DeptName + "%'";
                }
                else
                {
                    filters = "  where DeptName like '%" + objFilterModel.DeptName + "%'";
                }
            }


            var queryData = GetList(filters, ref errMsg);
            var Queryable = queryData.AsQueryable();
            var query = LinqHelper.DataSorting(Queryable, pager.sort, pager.order);
            return CreateModelList(ref pager, ref query);

            //IQueryable<mUser> queryData = null;
            //var queryData = GetList(filters, ref errMsg);
            //queryData = LinqHelper.DataSorting(queryData, pager.sort, pager.order);
            //return CreateModelList(ref pager, ref queryData);
        }

        private List<mUsersModel> CreateModelList(ref GridPager pager, ref IQueryable<mUsersModel> queryData)
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
                               select new mUsersModel
                               {
                                   id = k.id,
                                   UserCode = k.UserCode != null ? k.UserCode.Trim() : "",
                                   UserSeq = k.UserSeq != null ? k.UserSeq.Trim() : "",
                                   UserName = k.UserName != null ? k.UserName.Trim() : "",
                                   DeptName = k.DeptName != null ? k.DeptName.Trim() : "",
                                   Description = k.Description != null ? k.Description.Trim() : "",
                                   Password =k.Password!=null? k.Password.Trim():"",
                                   RoleName = k.RoleName!=null?k.RoleName.Trim():"",
                                   OrganizeName =k.OrganizeName!=null? k.OrganizeName.Trim():"",
                                   ConfigJSON = k.ConfigJSON!=null ?k.ConfigJSON.Trim():"",
                                   IsEnable = (k.IsEnable == "True" ? "有效" : "失效"),
                                   LoginCount =k.LoginCount!=null?k.LoginCount:0,
                                   LastLoginDate = k.LastLoginDate!=null?k.LastLoginDate:DateTime.Now,
                                   CreatePerson = k.CreatePerson != null ? k.CreatePerson.Trim() : "",
                                   CreateDate = k.CreateDate!=null?k.CreateDate:DateTime.Now,
                                   UpdatePerson = k.UpdatePerson!=null?k.UpdatePerson.Trim():"",
                                   UpdateDate = k.UpdateDate!=null?k.UpdateDate:DateTime.Now,

                               }).ToList();
                return lstData;
        }


        public object Login(JObject request)
        {
            var UserCode = request.Value<string>("usercode");
            var Password = request.Value<string>("password");
            //用户名密码检查
            if (String.IsNullOrEmpty(UserCode) || String.IsNullOrEmpty(Password))
                return new { status = "error", message = "用户名或密码不能为空！" };

            prjGeo.Models.mUser oUser  = objUser.Login(UserCode.ToString(), Password.ToString());
            if (oUser==null)   return new { status = "error", message = "用户已被注销，请联系管理员！" };

            UserInfo.strUserCode = oUser.UserCode.Trim();
            UserInfo.strUserName = oUser.UserName.Trim(); 
            UserInfo.blnIsEnable = oUser.IsEnable==false?false:true;
            UserInfo.strConfigJson = oUser.ConfigJSON == null ? "" : oUser.ConfigJSON.Trim();
            UserInfo.strDeptName = oUser.DeptName.Trim();

            if (oUser==null)
                return new { status = "error", message = "用户名或密码不正确！" };
            string strUserName = "";
            var loginer = new LoginerBase { UserCode = UserCode.ToString(), UserName = strUserName };
            var effectiveHours = BaseConfig.GetConfigInt("LoginEffectiveHours");
            FormsAuth.SignIn(loginer.UserCode, loginer, 60 * effectiveHours);
            this.UpdateUserLoginCountAndDate(UserCode); //更新用户登陆次数及时间

            return new { status = "success", message = "登陆成功！" };
        }
        public void UpdateUserLoginCountAndDate(string UserCode)
        {
             objUser.UpdateUserLoginCountAndDate(UserCode,db);
        }

        #region 添加修改删除操作
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Add(mUser model, ref string errMsg)
        {
            model.ConfigJSON = model.ConfigJSON == null ? "" : model.ConfigJSON.Trim();
            model.CreatePerson = model.CreatePerson == null ? UserInfo.strUserCode.Trim() : UserInfo.strUserCode.Trim();
            model.DeptName = model.DeptName == null ? "" : UserInfo.strDeptName.Trim();
            model.Description = model.Description == null ? "" : model.Description.Trim();
            model.IsEnable = model.IsEnable == null ? false : true;
            model.UserSeq = "0";
            model.UpdatePerson = UserInfo.strUserCode;
            model.UserCode = model.UserCode == null ? "" : model.UserCode.Trim();
            model.UserName = model.UserName == null ? "" : model.UserName.Trim();
            model.LastLoginDate = DateTime.Now;
            return objUser.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mUser model, ref string errMsg)
        {
            model.ConfigJSON = model.ConfigJSON == null ? "" : model.ConfigJSON.Trim();
            model.CreatePerson = model.CreatePerson == null ? UserInfo.strUserCode.Trim() : UserInfo.strUserCode.Trim();
            model.DeptName = model.DeptName == null ? "" : model.DeptName.Trim();
            model.Description = model.Description == null ? "" : model.Description.Trim();
            model.IsEnable = model.IsEnable == null ? false : true;
            model.UserSeq =model.UserSeq==null? "0":model.UserSeq;
            model.UpdatePerson = UserInfo.strUserCode;
            model.UserCode = model.UserCode == null ? "" : model.UserCode.Trim();
            model.UserName = model.UserName == null ? "" : model.UserName.Trim();
            model.LastLoginDate = DateTime.Now;
            return objUser.Update(model, db, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mUser model, ref string errMsg)
        {
            return objUser.Delete(model, db, ref errMsg);
        }

        #endregion



    }
}
