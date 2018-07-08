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
        mUsersDAL objUser = new mUsersDAL();
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

        public List<mUsersModel> GetIndexList(mUsersModel objFilterModel, ref string errMsg, ref GridPager pager)
        {
            IQueryable<mUser> queryData = null;
            queryData = objUser.GetAllList(db);
            queryData = LinqHelper.DataSorting(queryData, pager.sort, pager.order);
            return CreateModelList(ref pager, ref queryData);
        }

        private List<mUsersModel> CreateModelList(ref GridPager pager, ref IQueryable<mUser> queryData)
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

            List<mUsersModel> lstData = (from k in queryData
                                         select new mUsersModel
                                         {
                                             id = k.id,
                                             UserCode = k.UserCode.Trim(),
                                             UserSeq = k.UserSeq.Trim(),
                                             UserName = k.UserName.Trim(),
                                             Description = k.Description.Trim(),
                                             Password = k.Password.Trim(),
                                             RoleName = k.RoleName.Trim(),
                                             OrganizeName = k.OrganizeName.Trim(),
                                             ConfigJSON = k.ConfigJSON.Trim(),
                                             IsEnable = (k.IsEnable==true)?"有效":"失效",
                                             LoginCount = k.LoginCount,
                                             LastLoginDate = k.LastLoginDate,
                                             CreatePerson = k.CreatePerson.Trim(),
                                             CreateDate = k.CreateDate,
                                             UpdatePerson = k.UpdatePerson.Trim(),
                                             UpdateDate = k.UpdateDate,

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
            return 0;
            //return dal.Add(model, db, ref errMsg);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Update(mUser model, ref string errMsg)
        {
            return 0;
            //return dal.Update(model, db, ref errMsg);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public int Delete(mUser model, ref string errMsg)
        {
            return 0;
            //return dal.Delete(model, db, ref errMsg);
        }

        #endregion



    }
}
