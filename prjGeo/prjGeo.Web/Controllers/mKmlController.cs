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
using System.Diagnostics;



namespace prjGeo.Web.Controllers
{

    public class mKmlController : BaseController
    {
        private string errMsg = string.Empty;
        private string _kmlFolder = string.Empty;

        private mKmlBLL objBLL = new mKmlBLL();

        public ActionResult Index()
        {
            ViewBag.Title = "图层管理";
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

                //  string path = _kmlFolder +  model.PrjName + "\\" + model.FileName;
                string path = Server.MapPath("/") + _kmlFolder + "\\" + model.PrjName + "\\" + model.FileName;
                path = path.Replace(".kml", ".xml");
                Debug.WriteLine("path " + path);
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



        public ActionResult uploadKml()
        {

            try
            {
                string action = Request.Form["action"];  //普通参数获取
                string oldFile = Request.Form["oldFile"];//普通参数获取
                string p1 = Request.Form["kmlData"];  //普通参数获取

                mKml kmlData = JsonConvert.DeserializeObject<mKml>(p1);

                if (string.IsNullOrEmpty(_kmlFolder))
                {
                    _kmlFolder = ConfigurationManager.AppSettings["KMLFolder"];
                }

                string path = Server.MapPath("/") + _kmlFolder + "\\" + kmlData.PrjName + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }

                foreach (string upload in Request.Files.AllKeys)
                {
                    HttpPostedFileBase file = Request.Files[upload];   //file可能为null
                    string filename = System.IO.Path.GetFileName(file.FileName);

                    filename = filename.Replace(".kml", ".xml");
                    string desPath = path + filename;
                    Debug.WriteLine("desPath " + desPath);

                    file.SaveAs(desPath);
                    kmlData.KmlPath = "/" + _kmlFolder + "/" + kmlData.PrjName + "/" + filename;
                }


                if (action.Equals("new"))
                {

                    objBLL.Add(kmlData, ref errMsg);
                }
                else if (action.Equals("modify"))
                {
                    objBLL.Update(kmlData, ref errMsg);

                    if (!string.IsNullOrEmpty(oldFile))
                    {
                        oldFile = oldFile.Replace(".kml", ".xml");
                        string oldKml = Server.MapPath("/") + _kmlFolder + "\\" + kmlData.PrjName + "\\" + oldFile;
                        Debug.WriteLine("oldKml " + oldKml);
                        if (System.IO.File.Exists(oldKml))
                        {
                            System.IO.File.Delete(oldKml);
                        }
                    }
                }
                return Json(new { errMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult GetListByFilter(string PrjName, string PrjId, string LayerName, GridPager pager)
        {
            string filters = "";
            if (!string.IsNullOrEmpty(PrjName))
            {
                filters = "PrjName like '%" + PrjName + "%' ";
            }
            if (!string.IsNullOrEmpty(PrjId))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += " and  ProjId like '%" + PrjId + "%' ";
                }
                else
                {
                    filters = " ProjId like '%" + PrjId + "%' ";
                }

            }
            if (!string.IsNullOrEmpty(LayerName))
            {
                if (!string.IsNullOrEmpty(filters))
                {
                    filters += " and  LayerName like '%" + LayerName + "%' ";
                }
                else
                {
                    filters = " LayerName like '%" + LayerName + "%' ";
                }

            }
            var list = objBLL.GetIndexList(filters, ref errMsg, ref pager);
            var json = new
            {
                total = pager.totalRows,
                rows = list.ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }


}
