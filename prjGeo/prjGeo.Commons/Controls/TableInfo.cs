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

namespace prjGeo.Commons
{
    public class TableInfo
    {
        private GeoGisEntities db=new GeoGisEntities();

        public List<dynamic> GetDropList()
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            List<dynamic> dtList = con.Database.SqlQuery<dynamic>("select id as value,FormType as text  from mForm").ToList();

            return dtList;
        }
        /// <summary>
        /// 获取表格的信息
        /// </summary>
        /// <param name="frmID">表格的ID(唯一)</param>
        /// <param name="intViewStatus">显示的状态:
        ///                                 0:列表 
        ///                                 1:编辑</param>
        /// <returns></returns>
        public string GetGridColInfo(int? frmID,int intViewStatus)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            StringBuilder columns = new StringBuilder("[[");
             
            try
            {
                StringBuilder strSQL = new StringBuilder();
                if (intViewStatus == 1)  //编辑状态
                {
                    strSQL = strSQL.AppendFormat("SELECT * FROM mGrid where FormID={0} order by seqno", frmID);
                    
                }
                else  //列表显示状态
                {
                    strSQL = strSQL.AppendFormat("SELECT * FROM mGrid where FormID={0} order by FormID,seqno", frmID);
                }
                List<mGrid> dtList = con.Database.SqlQuery<mGrid>(strSQL.ToString()).ToList(); 
                if (dtList != null)
                {
                    if (intViewStatus == 1)  //编辑状态
                    {
                        foreach (mGrid dr in dtList)
                        {
                            columns = columns.AppendFormat("{{field:'{0}',title:'{1}',width:{2},align:'{3}',hidden:{4},rowspan:{5},sum:{6},avg:{7},editor:{8}}},",
                                      dr.ColField.ToString(),
                                      dr.ColTitle.ToString(),
                                      dr.ColWidth.ToString(),
                                      dr.ColAlign.ToString(),
                                      bool.Parse(dr.Colhidden.ToString()) == false ? "false" : "true",
                                      dr.ColRowspan.ToString(),
                                      bool.Parse(dr.isSum.ToString()) == false ? "false" : "true",
                                      bool.Parse(dr.isAvg.ToString()) == false ? "false" : "true",
                                      dr.ColEditor.ToString());
                        };
                    }
                    else   //列表显示状态
                    {
                        foreach (mGrid dr in dtList)
                        {
                            columns = columns.AppendFormat("{{field:'{0}',title:'{1}',width:{2},align:'{3}',hidden:{4},rowspan:{5},sum:{6},avg:{7}}},",
                                      dr.ColField.ToString(),
                                      dr.ColTitle.ToString(),
                                      dr.ColWidth.ToString(),
                                      dr.ColAlign.ToString(),
                                      bool.Parse(dr.Colhidden.ToString()) == false ? "false" : "true",
                                      dr.ColRowspan.ToString(),
                                      bool.Parse(dr.isSum.ToString()) == false ? "false" : "true",
                                      bool.Parse(dr.isAvg.ToString()) == false ? "false" : "true"
                                      );
                        };
                    }
                    columns.Remove(columns.Length - 1, 1);//去除多余的','号 
                    columns.Append("]]");
                }
                return columns.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JsGridColumn> GetInitGridCols(int formid)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            String strSQL = string.Empty;
            List<JsGridColumn> objGrid = new List<JsGridColumn>();
            objGrid.Add(new JsGridColumn() { fldType = "CHK", fldName = "CHK", Title = "" });
            objGrid.Add(new JsGridColumn() { fldType = "LBL", fldName = "ColTitle", Title = "列名" });
            objGrid.Add(new JsGridColumn() { fldType = "LBL", fldName = "ColField", Title = "字段" });
            return objGrid;
 
        }
        public List<JsGridRow> GetInitGridRows(int formid)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            String strSQL = string.Empty;
            strSQL = string.Format("SELECT * FROM mGrid where FormID={0} order by FormID,seqno", formid);
            List<mGrid> dtList = con.Database.SqlQuery<mGrid>(strSQL).ToList();
            if (dtList != null)
            {
                List<JsGridRow> objGrid = (from c in dtList select new JsGridRow() { fldType = "CHK", fldName = c.ColField, Title = c.ColTitle }).ToList();
                return objGrid;
            }
            else
            {
                return null;
            }
        }


        ///设置表单上的网格列是否可见
        //{  'rows': [{ 'Column1': '1A', 'Column2': '1B', 'Column3': '1C' }, { 'Column1': '2A', 'Column2': '2B', 'Column3': '2C' }] };
        public string GetColsInfo(int formid)
        {
            DbContext con = (DbContext)(db as IObjectContextAdapter);
            String strSQL = string.Empty;
            StringBuilder columns = new StringBuilder("{");


            try
            {
                strSQL = string.Format("SELECT * FROM mGrid where FormID={0} order by FormID,seqno", formid);
                List<mGrid> dtList = con.Database.SqlQuery<mGrid>(strSQL).ToList();
                columns = columns.Append("'columns':[{'FieldID':'CHK','Title':'CHK'},{'FieldID':'ColTitle','Title':'列名'},{'FieldID':'ColField','Title':'字段'}],'rows':[");
                if (dtList != null)
                {
                    List<GridColsInfo> objCols = (from c in dtList select new GridColsInfo() { ColName = c.ColTitle, ColField = c.ColField,ColHidd=c.Colhidden}).ToList();
                    foreach (GridColsInfo obj in objCols)
                    {
                        //columns = columns.AppendFormat("{field:'CHK',ColTitle:'{0}',ColField:'{1}'},",
                        columns = columns.Append("{");
                        columns = columns.AppendFormat("'CHK':'CHK','ColTitle':'{0}','ColField':'{1}'",
                            obj.ColName.ToString(),
                            obj.ColField.ToString()
                            );
                        columns = columns.Append("},");
                    };
                    columns.Remove(columns.Length - 1, 1);//去除多余的','号 
                    columns.Append("]}");

                    return columns.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
