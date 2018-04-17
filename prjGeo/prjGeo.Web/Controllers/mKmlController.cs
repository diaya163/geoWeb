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
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace prjGeo.Web.Controllers
{

    public class mKmlController : BaseController
    {
        private string errMsg = string.Empty;
        private string _kmlFolder = string.Empty;

        private mKmlBLL objBLL = new mKmlBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "分部维护";
            //List<mUsersModel> list =objUser.GetAllList(),
            var model = new
            {
                form = new
                {

                },
                GridInfo = new
                {
                    idField = "id",
                    ColInfo = new TableInfo().GetGridColInfo(120, 0),
                    sortName = "id"
                },
                GridColInfo = new
                {
                    columns = new TableInfo().GetInitGridCols(120),
                    rows = new TableInfo().GetInitGridRows(120)
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

        public ActionResult SaveData(string action, mKml model)
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
        public ActionResult Delete(mKml model)
        {
            try
            {
                //delete kml data
                objBLL.Delete(model, ref errMsg);

                //delete kml file
                if (string.IsNullOrEmpty(_kmlFolder))
                {
                    _kmlFolder = ConfigurationManager.AppSettings["KMLFolder"];
                }

                string path = _kmlFolder + "\\" + model.PrjName + "\\" + model.FileName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                }

                return Json(new { errMsg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }

           
           
            

        }
        public ActionResult GetListDataById(string id)
        {
            string strFilter = "id='" + id + "'";
            IList<mKml> list = objBLL.GetList(strFilter, ref errMsg);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListData()
        {
            var list = objBLL.GetList(string.Empty, ref errMsg);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 文件上传页面
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public ActionResult UploadifyFile(HttpPostedFileBase filedata)
        {
            if (filedata == null ||
              String.IsNullOrEmpty(filedata.FileName) ||
              filedata.ContentLength == 0)
            {
                return HttpNotFound();
            }

            string filename = System.IO.Path.GetFileName(filedata.FileName);
            if (string.IsNullOrEmpty(_kmlFolder))
            {
                _kmlFolder = ConfigurationManager.AppSettings["KMLFolder"];
            }

            //string virtualPath = String.Format("~/File/{0}", filename);

            // string path = Server.MapPath(virtualPath);

            string path = _kmlFolder + DateTime.Now.ToString() + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            path = path + filename;


            // 以下注释的代码 都可以获得文件属性
            // System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
            // FileInfo file = new FileInfo(filedata.FileName);

            filedata.SaveAs(path);
            return null;
        }


        public ActionResult uploadKml()
        {

            try
            {
                string action = Request.Form["action"];  //普通参数获取
                string p1 = Request.Form["kmlData"];  //普通参数获取
                mKml kmlData = JsonConvert.DeserializeObject<mKml>(p1);

                if (string.IsNullOrEmpty(_kmlFolder))
                {
                    _kmlFolder = ConfigurationManager.AppSettings["KMLFolder"];
                }

                if (action.Equals("new"))
                {
                    foreach (string upload in Request.Files.AllKeys)
                    {
                        HttpPostedFileBase file = Request.Files[upload];   //file可能为null
                        string filename = System.IO.Path.GetFileName(file.FileName);

                        string path = _kmlFolder + "\\" + kmlData.PrjName + "\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);

                        }
                        filename = filename.Replace(".kml", ".xml");
                        path = path + filename;
                        file.SaveAs(path);
                        kmlData.KmlPath = "/KMLFiles/" + kmlData.PrjName + "/" + filename;
                    }
                    objBLL.Add(kmlData, ref errMsg);
                }
                else if (action.Equals("modify"))
                {
                    foreach (string upload in Request.Files.AllKeys)
                    {
                        HttpPostedFileBase file = Request.Files[upload];   //file可能为null
                        string filename = System.IO.Path.GetFileName(file.FileName);

                        string path = _kmlFolder + "\\" + kmlData.PrjName + "\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);

                        }
                        filename = filename.Replace(".kml", ".xml");
                        path = path + filename;
                        file.SaveAs(path);
                        kmlData.KmlPath = "/KMLFiles/" + kmlData.PrjName + "/" + filename;
                    }
                    objBLL.Update(kmlData, ref errMsg);
                }
                return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
           
        }



 

    }


}
