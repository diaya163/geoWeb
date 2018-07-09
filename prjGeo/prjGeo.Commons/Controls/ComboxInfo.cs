using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjGeo.Models;
using prjGeo.Models.Sys;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Entity.Infrastructure;
using Esquel.Utility;

namespace prjGeo.Commons
{
    public class CmbModel
    {
        public int id { get; set; }
        public int FormID { get; set; }
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public string QrySQL { get; set; }

        public int IsSQL{get;set;}
        public string DefValues { get; set; }
    }
    public class DropModel
    {
        public string strKey { get; set; }
        public string strText { get; set; }

    }
    public class ComboxInfo
    {
        private readonly string strTable="mCmboxSet";
        private GeoGisEntities db = new GeoGisEntities();

        public List<DropModel> GetDropListEx(int formid, string strFldID)
        {
            if ((strFldID.Trim().Length == 0) || (strFldID.Trim().Length == 0))
                return null;

            DbContext con = (DbContext)(db as IObjectContextAdapter);
            string strSQL = string.Format("select top 1 * from " + strTable + " where formid={0} and FieldID='{1}'", formid, strFldID);
            List<CmbModel> list = con.Database.SqlQuery<CmbModel>(strSQL).ToList();

            List<DropModel> olist = null;
            if (list != null)
            {
                //list.Find(c => c.IsSQL.Equals(1));
                foreach (CmbModel objCmb in list)  //该循环只执行一次
                {
                    if (objCmb.IsSQL == 1)//使用SQL查询获取值
                    {

                        strSQL = "select distinct gecata as strKey,gecata as strText from U_HSAllSeq order by gecata";
                        olist = con.Database.SqlQuery<DropModel>(strSQL).ToList();

                    }
                    else//使用缺省值
                    {

                    }
                }
            }

            return olist;
        }

        public string GetDropList(int formid, string strFldID,string strMsgCmb,ref string errMsg)
        { 
            if ((strFldID.Trim().Length==0) ||(strFldID.Trim().Length==0)) 
                return null;
              
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            try
            {
                string strSQL = string.Format("select top 1 * from " + strTable + " where formid={0} and FieldID='{1}'", formid, strFldID);
                List<CmbModel> list = con.Database.SqlQuery<CmbModel>(strSQL).ToList();

                List<DropModel> olist = null;
                DropModel objDropModel = new DropModel { strKey = "", strText = "" };
                if (list != null)
                {
                    var objA = list.Find(c => c.IsSQL.Equals(1));

                    strSQL = objA.QrySQL.ToString().Trim();
                    olist = con.Database.SqlQuery<DropModel>(strSQL).ToList();
                    if (strMsgCmb.Length>0)
                    { olist.Insert(0, new DropModel { strKey = "MsgCmb", strText = strMsgCmb.Trim() }); }
                    
                    // olist.Add(new DropModel { strKey = "", strText = "" });
                    #region 调试代码
                    //strSQL = "select distinct gecata as strKey,gecata+'类' as strText from U_HSAllSeq order by gecata";// 
                    //foreach (CmbModel objCmb in list)  //该循环只执行一次
                    //{
                    //    if (objCmb.IsSQL == 1)//使用SQL查询获取值
                    //    {

                    //        strSQL = "select distinct gecata as Value,gecata as Text from U_HSAllSeq order by gecata"; //objCmb.QrySQL.ToString();
                    //        olist = con.Database.SqlQuery<DropModel>(strSQL).ToList();

                    //    }
                    //    else//使用缺省值
                    //    { 

                    //    }
                    //}
                    #endregion
                }
                string strJson = DropToJson(olist);

                return strJson;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
            finally
            {
                con.Dispose(); 
            }
        }

        private string DropToJson(List<DropModel> objs)
        {
            if (objs == null) return null;

            StringBuilder json = new StringBuilder();
 
            foreach (DropModel item in objs)
            {
 
                if (json.Length == 0)
                {
                    json.Append("{ \"id\":\"" + item.strKey + "\",\"text\":\"" + item.strText + "\"},");
                }
                else
                {
                    json.Append("{ \"id\":\"" + item.strKey + "\",\"text\":\"" + item.strText + "\"},");
                }
            }
            json.Remove(json.Length - 1, 1);
            string reText = "[" + json.ToString() + "]";
            return reText;
        }

    }
}
