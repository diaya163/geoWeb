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
    public class QryModel
    {
        public int id { get; set; }
        public int FormID { get; set; }
        public string QrySQL { get; set; }
        public string QryFlds { get; set; }
        public char chrSeq { get; set; }
    }
    public class FiterCond
    {
        private readonly string strTable = "mQuery";
        private GeoGisEntities db = new GeoGisEntities();
        public string GetFiterCond(int id, object objModel, ref string errMsg)
        {
            if (objModel == null) return "";
            string strCondition = string.Empty;

            DbContext con = (DbContext)(db as IObjectContextAdapter);
            try
            {
                string strSQL = string.Format("select top 1 * from " + strTable + " where   id='{0}'",  id);
                List<QryModel> list = con.Database.SqlQuery<QryModel>(strSQL).ToList();
 
                if (list != null)
                {
                    var objA = list.Find(c => c.QrySQL !=null);                      
                    strSQL = objA.QrySQL.ToString().Trim();
                    string strFiter = objA.QryFlds.ToString().Trim();
                    char chrSeq = char.Parse(objA.chrSeq.ToString().Trim());
                    string[] strArr = strFiter.Split('|');
                    foreach (string str in strArr)
                    {
                        if (chkIsExistPropty(objModel, str ))
                        {
                            strCondition = "  where " + str;
                        }

                    }
    
 
                }
                return "";
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
            finally
            {
                con.Dispose(); 
            }
             
        }

        private static bool chkIsExistPropty<T>(T t, string strFind = "")
        {
            string tStr = string.Empty;
            if (t == null || strFind == "")
            {
                return false;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return false;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (name.Trim().ToUpper().Equals(strFind.Trim().ToUpper()))
                {
                    return true;                    
                }
            }
            return false;
        }




        //遍历获取类的属性及属性的值：
        private static string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1},", name, value);
                }
                else
                {
                    getProperties(value);
                }
            }
            return tStr;
        }

        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            System.Reflection.PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;

            return o.ToString();
        }
    }
}
