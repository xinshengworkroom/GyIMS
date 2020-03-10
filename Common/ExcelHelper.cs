using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace GyIMS.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// 导出Execl (放入HttpResponse)
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="FileName"></param>
        public static void CreateExcel(DataTable datatable, string FileName)
        {
            HttpContext curContext = HttpContext.Current;
            HttpResponse resp;
            resp = curContext.Response;
            string encodefileName = ToHexString(FileName);
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + encodefileName + ".xls");
            string colHeaders = "", ls_item = "";

            //定义表对象与行对象，同时用DataSet对其值进行初始化
            DataTable dt = datatable;
            DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的
            int i = 0;
            int cl = dt.Columns.Count;

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            for (i = 0; i < cl; i++)
            {
                if (i == (cl - 1))//最后一列，加\n
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                }
                else
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                }

            }
            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息

            //逐行处理数据  
            foreach (DataRow row in myRow)
            {
                //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据    
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))//最后一列，加\n
                    {
                        ls_item += row[i].ToString() + "\n";
                    }
                    else
                    {
                        ls_item += row[i].ToString() + "\t";
                    }

                }
                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        public static void CreateExcel<T>(List<T> list, string FileName)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            foreach (PropertyInfo p in props)
            {
                DisplayNameAttribute display = p.GetCustomAttribute<DisplayNameAttribute>();
                if (display != null)
                {
                    string colName = display.DisplayName;
                    if (dt.Columns[display.DisplayName] != null)
                        colName += "1";
                    dt.Columns.Add(colName);
                }
            }

            if (list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        DisplayNameAttribute display = pi.GetCustomAttribute<DisplayNameAttribute>();
                        if (display != null)
                        {
                            object obj = pi.GetValue(list.ElementAt(i), null);
                            tempList.Add(obj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }

            CreateExcel(dt, FileName);

        }



        /// <summary>
        /// excel转换为datatable
        /// </summary>
        /// <param name="strExcelFileName">文件路径</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strExcelFileName, string sheetName = "", int sheetId = 1)
        {

            //源的定义excel 2010   HDR=No 首列暂时不设为列名
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelFileName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
            //Sql语句    
            //string strExcel = "select * from [sheet1$]";

            //连接数据源
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = "";
            DataSet ds = new DataSet();
            if (sheetId == 1)
            {
                //如果sheet的名称为空，那么使用默认sheet
                if (string.IsNullOrEmpty(sheetName))
                {
                    tableName = schemaTable.Rows[0][2].ToString().Trim();
                }
                else
                {
                    tableName = sheetName;
                }

                string strExcel = string.Format("select * from [{0}]", tableName);

                //适配到数据源
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, conn);
                adapter.Fill(ds, tableName);

                conn.Close();
            }
            else if (sheetId > 1)
            {
                if (schemaTable.Rows.Count > 1)
                {
                    //如果sheet的名称为空，那么使用对应sheetId的sheet
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        tableName = schemaTable.Rows[sheetId - 1][2].ToString().Trim();
                    }
                    else
                    {
                        tableName = sheetName;
                    }

                    string strExcel = string.Format("select * from [{0}]", tableName);

                    //适配到数据源
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                    adapter.Fill(ds, tableName);

                    conn.Close();
                }
                else return null;
            }

            var dt = ds.Tables[tableName];
            return ConvertDataTableColumnName(dt);
        }

        /// <summary>
        /// 去除列名为空列（及第一行为空的列），并将第一行转换成列名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DataTable ConvertDataTableColumnName(DataTable dt)
        {
            List<string> columnList = new List<string>();
            //获取第一行（列名行） 不为空的列名
            foreach (DataColumn column in dt.Columns)
            {
                if (dt.Rows[0][column.ColumnName] != null &&
                    !string.IsNullOrEmpty(dt.Rows[0][column.ColumnName].ToString()))
                {
                    string columnName = dt.Rows[0][column.ColumnName].ToString();
                    column.ColumnName = columnName;
                    columnList.Add(columnName);
                }
            }
            //获取对应的列名
            DataTable dataTable = dt.DefaultView.ToTable(false, columnList.ToArray());
            //删除第一行
            dataTable.Rows.RemoveAt(0);
            return dataTable;
        }

        /// <summary>
        /// 获取对应excel的列名
        /// </summary>
        /// <param name="strExcelFileName">文件路径</param>
        /// <param name="sheetId"></param>
        /// <returns></returns>
        public static List<string> GetSheetList(string strExcelFileName)
        {
            //源的定义excel 2010
            string strConn = "Provider= Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelFileName + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";

            //连接数据源
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

            return (from DataRow columns in schemaTable.Rows
                    select columns["TABLE_NAME"].ToString()).ToList();
        }


        /// <summary>
        /// 获取对应excel的列名
        /// </summary>
        /// <param name="strExcelFileName">文件路径</param>
        /// <param name="sheetId"></param>
        /// <returns></returns>
        public static IList<string> GetColumnNameFromExcel(string strExcelFileName, string sheetName = "", int sheetId = 1)
        {
            //源的定义excel 2003
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

            //源的定义excel 2010
            string strConn = "Provider= Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelFileName + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";

            //连接数据源
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

            string tableName = "";
            IList<string> list = new List<string>();

            if (sheetId == 1)
            {
                //如果sheet的名称为空，那么使用默认sheet
                if (string.IsNullOrEmpty(sheetName))
                {
                    tableName = schemaTable.Rows[sheetId - 1][2].ToString().Trim();
                }
                else
                {
                    tableName = sheetName;
                }

                System.Data.DataTable tableColumns
                    = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                foreach (System.Data.DataRow drowColumns in tableColumns.Rows)
                {
                    if (drowColumns["Column_Name"].ToString() != "F1")
                        list.Add(drowColumns["Column_Name"].ToString());
                }

                conn.Close();
            }
            else if (sheetId > 1)
            {
                if (schemaTable.Rows.Count > 1)
                {
                    //如果sheet的名称为空，那么使用对应sheetId的sheet
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        tableName = schemaTable.Rows[sheetId - 1][2].ToString().Trim();
                    }
                    else
                    {
                        tableName = sheetName;
                    }

                    System.Data.DataTable tableColumns
                        = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                    foreach (System.Data.DataRow drowColumns in tableColumns.Rows)
                    {
                        list.Add(drowColumns["Column_Name"].ToString());
                    }

                    conn.Close();
                }
                else return null;
            }

            return list;
        }

        /// <summary>
        /// Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }

            return builder.ToString();
        }
    }
}