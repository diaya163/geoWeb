using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using prjGeo.Commons;
using prjGeo.BLL;
using prjGeo.Models;
using prjGeo.Models.Sys;
using System.IO;

namespace prjGeo.Web.Controllers
{
    public class mElementController : BaseController
    {
        private string errMsg = string.Empty;

        private mElementBLL objBLL = new mElementBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "基本元素维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(111, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(111),
                    rows = new TableInfo().GetInitGridRows(111)
                }


            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetList(GridPager pager)
        {
            string filters = string.Empty;
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData(string action, mElement model)
        {
            if (action.Equals("new"))
            {
                objBLL.Add(model, ref errMsg);
            }
            else if (action.Equals("modify"))
            {
                objBLL.Update(model, ref errMsg);
            }
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(mElement model)
        {
            objBLL.Delete(model, ref errMsg);
            return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将文件上传到指定路径中保存
        /// </summary>
        /// <returns>上传文件结果信息</returns>
        [HttpPost]
        [ValidateInput(false)]
        public string PostExcelData()
        {
            string info = string.Empty;
            errMsg = string.Empty;
            try
            {
                //获取客户端上传的文件集合
                string id = System.Web.HttpContext.Current.Request.QueryString["id"];

                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                //判断是否存在文件
                if (files.Count > 0)
                {
                    //获取文件集合中的第一个文件(每次只上传一个文件)
                    HttpPostedFile file = files[0];
                    //定义文件存放的目标路径
                    string targetDir = System.Web.HttpContext.Current.Server.MapPath("~/FileUpLoad/Element");
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    //组合成文件的完整路径
                    string path = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(file.FileName));

                    file.SaveAs(path);
                    mElement model = new mElement();
                    model.id = Convert.ToInt32(id);
                    model.Attachment = @"http://" + Request.Url.Authority + "/FileUpLoad/Element/" + file.FileName;
                    objBLL.UpdateAttachment(model, ref errMsg);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        System.IO.File.Delete(path);
                        info = errMsg;
                    }
                    else
                    {
                        info = "上传成功";
                    }
                }
                else
                {
                    info = "上传失败";
                }
            }
            catch (Exception ex)
            {
                info = "上传失败" + ex.Message;
            }
            return info;
        }
    }
}
