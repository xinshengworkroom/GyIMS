using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GyIMS.App_Helper
{
    public static class NpoiExtend
    {

        #region 读取Excel数据
        //导入获取DataTable
        public static DataTable Import(string strFileName)
        {
            try
            {

                DataTable dt = new DataTable();
                HSSFWorkbook hssfworkbook;
                XSSFWorkbook xssfworkbook;
                string extension = Path.GetExtension(strFileName);
                if (extension == ".xls")
                {
                    using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                    {
                        hssfworkbook = new HSSFWorkbook(file);
                    }
                    HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                    HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                        dt.Columns.Add(cell.ToString());
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        HSSFRow row = (HSSFRow)sheet.GetRow(i);
                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        dataRow[j] = null;
                                        break;
                                    case CellType.Boolean:
                                        dataRow[j] = cell.BooleanCellValue;
                                        break;
                                    case CellType.Numeric:
                                        dataRow[j] = cell.ToString();
                                        short format = cell.CellStyle.DataFormat;
                                        if (format == 14 || format == 31 || format == 57 || format == 58)
                                        {

                                            DateTime date = cell.DateCellValue;
                                            string re = date.ToString("yyyy-MM-dd HH:mm:ss");
                                            dataRow[j] = re;
                                        }
                                        else
                                        {
                                            if (cell.NumericCellValue.ToString2().Contains("E"))
                                            {
                                                dataRow[j] = Extension.ChangeToDecimal(cell.NumericCellValue.ToString2());
                                            }
                                            else
                                            {
                                                dataRow[j] = cell.NumericCellValue;
                                            }
                                        }
                                        break;
                                    case CellType.String:
                                        dataRow[j] = cell.StringCellValue.Trim2();
                                        break;
                                    case CellType.Error:
                                        dataRow[j] = cell.ErrorCellValue;
                                        break;
                                    case CellType.Formula:
                                    default:
                                        dataRow[j] = "=" + cell.CellFormula;
                                        break;
                                }
                        }
                        bool no_null = false;
                        for (int k = 0; k < cellCount; k++)
                        {

                            if (!string.IsNullOrEmpty(dataRow[k].ToString().Trim()))
                            {
                                no_null = true;
                            }

                        }
                        if (no_null)
                        {
                            dt.Rows.Add(dataRow);
                        }

                    }

                }
                if (extension == ".xlsx")
                {
                    using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                    {
                        xssfworkbook = new XSSFWorkbook(file);
                    }
                    XSSFSheet sheet = (XSSFSheet)xssfworkbook.GetSheetAt(0);
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                    XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        XSSFCell cell = (XSSFCell)headerRow.GetCell(j);
                        dt.Columns.Add(cell.ToString());
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        XSSFRow row = (XSSFRow)sheet.GetRow(i);
                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        dataRow[j] = null;
                                        break;
                                    case CellType.Boolean:
                                        dataRow[j] = cell.BooleanCellValue;
                                        break;
                                    case CellType.Numeric:
                                        dataRow[j] = cell.ToString();
                                        short format = cell.CellStyle.DataFormat;
                                        if (format == 14 || format == 31 || format == 57 || format == 58)
                                        {

                                            DateTime date = cell.DateCellValue;
                                            string re = date.ToString("yyyy-MM-dd HH:mm:ss");
                                            dataRow[j] = re;
                                        }
                                        else
                                        {
                                            if (cell.NumericCellValue.ToString2().Contains("E"))
                                            {
                                                dataRow[j] = Extension.ChangeToDecimal(cell.NumericCellValue.ToString2());
                                            }
                                            else
                                            {
                                                dataRow[j] = cell.NumericCellValue;
                                            }
                                        }
                                        break;
                                    case CellType.String:
                                        dataRow[j] = cell.StringCellValue.Trim2();
                                        break;
                                    case CellType.Error:
                                        dataRow[j] = cell.ErrorCellValue;
                                        break;
                                    case CellType.Formula:
                                    default:
                                        dataRow[j] = "=" + cell.CellFormula;
                                        break;
                                }
                        }
                        bool no_null = false;
                        for (int k = 0; k < cellCount; k++)
                        {

                            if (!string.IsNullOrEmpty(dataRow[k].ToString().Trim()))
                            {
                                no_null = true;
                            }

                        }
                        if (no_null)
                        {
                            dt.Rows.Add(dataRow);
                        }
                    }
                }
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //导入按sheet 顺序读取获取DataSet
        public static DataSet Import_DataSet(string strFileName)
        {
            try
            {

                DataSet ds = new DataSet();
                HSSFWorkbook hssfworkbook;
                XSSFWorkbook xssfworkbook;
                string extension = Path.GetExtension(strFileName);
                if (extension == ".xls")
                {
                    using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                    {
                        hssfworkbook = new HSSFWorkbook(file);
                    }
                    int sheetnums = hssfworkbook.NumberOfSheets;
                    for (int n = 0; n < sheetnums; n++)
                    {
                        DataTable dt = new DataTable("" + n + "");
                        HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(n);
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                        HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            HSSFRow row = (HSSFRow)sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                ICell cell = row.GetCell(j);
                                if (cell != null)
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = null;
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = cell.BooleanCellValue;
                                            break;
                                        case CellType.Numeric:
                                            dataRow[j] = cell.ToString();
                                            short format = cell.CellStyle.DataFormat;
                                            if (format == 14 || format == 31 || format == 57 || format == 58)
                                            {

                                                DateTime date = cell.DateCellValue;
                                                string re = date.ToString("yyyy-MM-dd HH:mm:ss");
                                                dataRow[j] = re;
                                            }
                                            else
                                            {
                                                if (cell.NumericCellValue.ToString2().Contains("E"))
                                                {
                                                    dataRow[j] = Extension.ChangeToDecimal(cell.NumericCellValue.ToString2());
                                                }
                                                else
                                                {
                                                    dataRow[j] = cell.NumericCellValue;
                                                }
                                            }
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue.Trim2();
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = cell.ErrorCellValue;
                                            break;
                                        case CellType.Formula:
                                        default:
                                            dataRow[j] = "=" + cell.CellFormula;
                                            break;
                                    }
                            }
                            bool no_null = false;
                            for (int k = 0; k < cellCount; k++)
                            {

                                if (!string.IsNullOrEmpty(dataRow[k].ToString().Trim()))
                                {
                                    no_null = true;
                                }

                            }
                            if (no_null)
                            {
                                dt.Rows.Add(dataRow);
                            }

                        }
                        ds.Tables.Add(dt);
                    }

                }
                if (extension == ".xlsx")
                {
                    using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                    {
                        xssfworkbook = new XSSFWorkbook(file);
                    }
                    int sheetnums = xssfworkbook.NumberOfSheets;
                    for (int n = 0; n < sheetnums; n++)
                    {
                        DataTable dt = new DataTable("" + n + "");
                        XSSFSheet sheet = (XSSFSheet)xssfworkbook.GetSheetAt(n);
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            XSSFCell cell = (XSSFCell)headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            XSSFRow row = (XSSFRow)sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                ICell cell = row.GetCell(j);
                                if (cell != null)
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = null;
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = cell.BooleanCellValue;
                                            break;
                                        case CellType.Numeric:
                                            dataRow[j] = cell.ToString();
                                            short format = cell.CellStyle.DataFormat;
                                            if (format == 14 || format == 31 || format == 57 || format == 58)
                                            {

                                                DateTime date = cell.DateCellValue;
                                                string re = date.ToString("yyyy-MM-dd HH:mm:ss");
                                                dataRow[j] = re;
                                            }
                                            else
                                            {
                                                if (cell.NumericCellValue.ToString2().Contains("E"))
                                                {
                                                    dataRow[j] = Extension.ChangeToDecimal(cell.NumericCellValue.ToString2());
                                                }
                                                else
                                                {
                                                    dataRow[j] = cell.NumericCellValue;
                                                }
                                            }
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue.Trim2();
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = cell.ErrorCellValue;
                                            break;
                                        case CellType.Formula:
                                        default:
                                            dataRow[j] = "=" + cell.CellFormula;
                                            break;
                                    }
                            }
                            bool no_null = false;
                            for (int k = 0; k < cellCount; k++)
                            {

                                if (!string.IsNullOrEmpty(dataRow[k].ToString().Trim()))
                                {
                                    no_null = true;
                                }

                            }
                            if (no_null)
                            {
                                dt.Rows.Add(dataRow);
                            }
                        }
                        ds.Tables.Add(dt);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //导入按页数获取DataTable
        public static DataTable Import_dt(string strFileName, int k)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            XSSFWorkbook xssfworkbook;
            string extension = Path.GetExtension(strFileName);
            if (extension == ".xls")
            {
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(k);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());
                }

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    if (row != null)
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }

                        dt.Rows.Add(dataRow);

                    }
                }

            }
            if (extension == ".xlsx")
            {
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    xssfworkbook = new XSSFWorkbook(file);
                }
                XSSFSheet sheet = (XSSFSheet)xssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    XSSFCell cell = (XSSFCell)headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());
                }

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    XSSFRow row = (XSSFRow)sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }

                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }
        //获取文档对象
        public static IWorkbook Get_WorkBook(string path)
        {

            IWorkbook _workbook;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (path.IndexOf(".xlsx") == -1)//2003
                    {
                        _workbook = new HSSFWorkbook(fileStream);

                    }
                    else//2007
                    {
                        _workbook = new XSSFWorkbook(fileStream);

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return _workbook;
        }
        //获取sheet名字
        public static string Get_sheet_name(string path)
        {

            IWorkbook _workbook;
            string sheet_name = string.Empty;
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (path.IndexOf(".xlsx") == -1)//2003
                    {
                        _workbook = new HSSFWorkbook(fileStream);
                        sheet_name = _workbook.GetSheetName(0);

                    }
                    else//2007
                    {
                        _workbook = new XSSFWorkbook(fileStream);
                        sheet_name = _workbook.GetSheetName(0);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sheet_name;
        }
        //保存文件
        public static string Save_Files(MemoryStream ms, string newpath)
        {

            try
            {
                string fileGuid = Guid.NewGuid().ToString();
                string excel_path = newpath + "\\" + fileGuid + ".xls";
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                using (FileStream fs = new FileStream(excel_path, FileMode.Create))
                {

                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {

                        byte[] bt = ms.ToArray();
                        bw.Write(bt, 0, bt.Length);
                    }
                }

                return excel_path;
            }
            catch (Exception)
            {
                throw;
            }
        }



        //获取第一个工作薄
        public static ISheet Get_sheet(string strFileName)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            XSSFWorkbook xssfworkbook;
            ISheet hhsf_xhsf_sheet = null;
            string extension = Path.GetExtension(strFileName);
            if (extension == ".xls")
            {
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                hhsf_xhsf_sheet = hssfworkbook.GetSheetAt(0);

            }
            if (extension == ".xlsx")
            {
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    xssfworkbook = new XSSFWorkbook(file);
                }

                hhsf_xhsf_sheet = xssfworkbook.GetSheetAt(0);
            }
            return hhsf_xhsf_sheet;
        }
        //插入行并复制样式
        public static void InsertRow(IWorkbook workbook, ISheet sheet, int insertRowIndex, int insertRowCount, IRow formatRow, int fristcell, int endcell)
        {
            //加边框样式
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.WrapText = true; //自动换行
            //插入原始行并给单元格加边框
            sheet.ShiftRows(insertRowIndex, sheet.LastRowNum, insertRowCount, true, false);
            for (int i = insertRowIndex; i <= insertRowCount + insertRowIndex; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = fristcell; j <= endcell; j++)
                {
                    ICell Cell = row.CreateCell(j);
                    Cell.CellStyle = style;
                }
            }
        }
        #endregion
        #region Excel 导出方法
        public static MemoryStream ExportDataTableToExcel(DataTable sourceTable, string sheetName)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            var sheet = workbook.CreateSheet(sheetName);
            int startrow = 0;
            foreach (DataRow row in sourceTable.Rows)
            {
                if (startrow == 0)
                {
                    var headerRow = sheet.CreateRow(0);
                    IFont font = workbook.CreateFont();
                    font.IsBold = true;
                    font.FontHeightInPoints = 11;
                    ICellStyle headStyle = workbook.CreateCellStyle();
                    headStyle.SetFont(font);
                    for (int i = 0; i < sourceTable.Columns.Count; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(sourceTable.Columns[i].ColumnName);
                        headerRow.GetCell(i).CellStyle = headStyle;
                    }
                    startrow = 1;
                }
                IRow dataRow = (HSSFRow)sheet.CreateRow(startrow);
                for (int i = 0; i < sourceTable.Columns.Count; i++)
                {
                    if (sourceTable.Columns[i].DataType.ToString() == "System.Decimal" ||
                        sourceTable.Columns[i].DataType.ToString() == "System.Int64" ||
                        sourceTable.Columns[i].DataType.ToString() == "System.Double")
                    {
                        dataRow.CreateCell(i).SetCellValue(double.Parse(row[i].ToString() == "" ? "0" : row[i].ToString()));
                    }
                    else
                    {
                        dataRow.CreateCell(i).SetCellValue(row[i].ToString());
                    }
                }
                startrow++;
            }

            workbook.Write(ms);
            var bt = ms.ToArray();
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        public static string DownloadExcel_Ms(MemoryStream ms, string fileName)
        {
            string virtualPath = string.Format("~/Resource/{0}", fileName);
            string path = System.Web.HttpContext.Current.Server.MapPath(virtualPath);
            saveTofle(ms, path);
            return path;
        }
        public static void DownloadPdf_Ms(string FullFileName, string newfilename)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + newfilename);
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.IO.File.Delete(FullFileName);
        }
        public static void Download_FullFileName(string FullFileName, string newfilename)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + newfilename);
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion


        #region CLEDB 转换为DataSet
        /// <summary>
        /// Oledb转换成DataSet
        /// </summary>
        public static DataSet ExcelToDataSet(string filepath, string strType, string[] sheets)
        {
            var ds = new DataSet();
            if (System.IO.File.Exists(filepath))
            {
                string strCon = string.Empty;
                if (strType == ".xlsx")
                {
                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;IMEX=1'";
                }
                else if (strType == ".xls")
                {
                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;IMEX=1'";
                }
                OleDbConnection conn = new OleDbConnection(strCon);
                conn.Open();
                try
                {
                    string sql = "select * from ";
                    foreach (var sheet in sheets)
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(sql + sheet, conn);
                        adapter.Fill(ds, sheet);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return ds;
        }
        #endregion
        public static void saveTofle(MemoryStream file, string fileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            }
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffer = file.ToArray();//转化为byte格式存储
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                buffer = null;
            }


        }



        //将datatable转化为文件流并保存到服务器端
        public static string savetohttp(DataTable dt, string url, string sheetname)
        {
            Stream s = NpoiExtend.ExportDataTableToBasicExcel(dt, sheetname);
            MemoryStream reqStream = (MemoryStream)s;
            string filepath = NpoiExtend.Save_Files(reqStream, System.Web.HttpContext.Current.Server.MapPath("~/Resource/ErrorExportInforMation"));
            //string path = System.Web.HttpContext.Current.Server.MapPath(filepath);           
            //string test = url + urlConvertorLocal(filepath);
            return url + urlConvertor(filepath);

        }
        /// <summary>
        /// 绝对路径转相对路径
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string urlConvertor(string strUrl)
        {
            string tmpRootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string urlPath = strUrl.Replace(tmpRootDir, ""); //转换成相对路径
            urlPath = urlPath.Replace(@"/", @"/");
            return urlPath;
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        private static string urlConvertorLocal(string strUrl)
        {
            string tmpRootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string urlPath = tmpRootDir + strUrl.Replace(@"/", @"/"); //转换成绝对路径
            return urlPath;
        }


        //yangyang.zhao/npoihelper
        #region 由DataSet、DataTable导出Excel
        /// <summary>
        /// 由DataSet导出Excel,被外界调用的方法
        /// </summary>   
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">指定Excel工作表名称</param>
        /// <param name="fileName">strType=0:普通格式 1有格式化的形式</param>
        /// <returns>Excel工作表</returns>    
        public static void ExportDataSetToExcel(DataSet sourceDs, string fileName, string sheetName, string strType)
        {
            MemoryStream ms = null;
            if (strType == "0")
            {
                ms = ExportDataSetToBasicExcel(sourceDs, sheetName) as MemoryStream;
            }
            else
            {
                ms = ExportDataSetToFormatExcel(sourceDs, sheetName) as MemoryStream;
            }
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }

        /// <summary>
        /// 由DataSet导出Excel(基本形式)
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>    
        public static Stream ExportDataSetToBasicExcel(DataSet sourceDs, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries); //分割符
            for (int i = 0; i < sheetNames.Length; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i]);

                // handling value.            
                int rowIndex = 0;
                int sheetnum = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    #region 创建表头
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheetnum++;
                            sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i] + "-" + sheetnum.ToString());
                        }
                        var headerRow = sheet.CreateRow(0);
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in sourceDs.Tables[i].Columns)
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        rowIndex = 1;
                    }
                    #endregion

                    #region 创建内容
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                    #endregion
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 由DataSet导出Excel(带有格式)
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>    
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private static Stream ExportDataSetToFormatExcel(DataSet sourceDs, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries); //分割符
            for (int i = 0; i < sheetNames.Length; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i]);


                var dateStyle = workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                //取得列宽  
                int[] arrColWidth = new int[sourceDs.Tables[i].Columns.Count];
                foreach (DataColumn item in sourceDs.Tables[i].Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int k = 0; k < sourceDs.Tables[i].Rows.Count; k++)
                {
                    for (int j = 0; j < sourceDs.Tables[i].Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(sourceDs.Tables[i].Rows[k][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }

                int rowIndex = 0;
                int sheetnum = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    #region 创建表头
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheetnum++;
                            sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i] + "-" + sheetnum.ToString());
                        }
                        var headerRow = sheet.CreateRow(0);
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        //设置边框
                        headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.BottomBorderColor = HSSFColor.Black.Index;
                        headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.LeftBorderColor = HSSFColor.Green.Index;
                        headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.RightBorderColor = HSSFColor.Blue.Index;
                        headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        headStyle.TopBorderColor = HSSFColor.Orange.Index;
                        //设置背景色
                        headStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Lime.Index;
                        headStyle.FillPattern = NPOI.SS.UserModel.FillPattern.BigSpots;
                        headStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;

                        foreach (DataColumn column in sourceDs.Tables[i].Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽  
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        rowIndex = 1;
                    }
                    #endregion

                    #region 创建内容
                    var dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        var newCell = dataRow.CreateCell(column.Ordinal);

                        string drValue = row[column].ToString();

                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型  
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型  
                            case "MySql.Data.Types.MySqlDateTime": //MySql类型
                                if (drValue == "0000/0/0 0:00:00" || String.IsNullOrEmpty(drValue))
                                {
                                    //当时间为空，防止生成的execl 中是一串“#######”号，所有赋值为空字符串
                                    newCell.SetCellValue("");
                                }
                                else
                                {
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示  
                                }
                                break;
                            case "System.Boolean"://布尔型  
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型  
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型  
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理  
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }
                    }
                    #endregion
                    rowIndex++;
                }
                //设置首行首列冻结
                //第一个参数表示要冻结的列数
                //第二个参数表示要冻结的行数
                //第三个参数表示右边区域可见的首列序号，从1开始计算
                //第四个参数表示下边区域可见的首行序号，也是从1开始计算
                sheet.CreateFreezePane(1, 1, 0, 10);
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel,基本方法
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param> 
        /// <returns>Excel工作表</returns>    
        public static Stream ExportDataTableToBasicExcel(DataTable sourceTable, string sheetName)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            NpoiMemoryStream ms = new NpoiMemoryStream();
            var sheet = workbook.CreateSheet(sheetName);
            int rowIndex = 0;
            int sheetnum = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                #region 创建表头
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheetnum++;
                        sheet = workbook.CreateSheet(sheetName + "-" + sheetnum.ToString());
                    }
                    var headerRow = sheet.CreateRow(0);
                    var headStyle = workbook.CreateCellStyle();
                    headStyle.Alignment = HorizontalAlignment.Center;
                    var font = workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    headStyle.SetFont(font);
                    foreach (DataColumn column in sourceTable.Columns)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    rowIndex = 1;
                }
                #endregion

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }
            ms.AllowClose = false;
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            workbook = null;
            return ms;
        }


        /// <summary>
        /// 带格式化的
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="sheetName">创建的Sheet名称</param>
        /// <returns></returns>
        private static Stream ExportDataTableToFormatExcel(DataTable dtSource, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽  
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            int rowIndex = 0;

            int sheetnum = 1;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheetnum++;
                        sheet = workbook.CreateSheet(sheetName + "-" + sheetnum.ToString());
                    }

                    #region 列头及样式
                    {
                        var headerRow = sheet.CreateRow(0);
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        //设置背景色
                        headStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
                        headStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽  
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    #endregion
                    rowIndex = 1;
                }
                #endregion


                #region 填充内容
                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {

                    var newCell = dataRow.CreateCell(column.Ordinal);
                    var contentStyle = workbook.CreateCellStyle();
                    contentStyle.Alignment = HorizontalAlignment.Center;
                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型  
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型  
                        case "MySql.Data.Types.MySqlDateTime": //MySql类型
                            if (drValue == "0000/0/0 0:00:00" || String.IsNullOrEmpty(drValue))
                            {
                                //当时间为空，防止生成的execl 中是一串“#######”号，所有赋值为空字符串
                                newCell.SetCellValue("");
                            }
                            else
                            {
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);

                                newCell.CellStyle = dateStyle;//格式化显示  
                            }
                            break;
                        case "System.Boolean"://布尔型  
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型  
                        case "System.Int32":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Int64":
                            Int64 int64V = 0;
                            Int64.TryParse(drValue, out int64V);
                            newCell.SetCellValue(int64V);
                            break;
                        case "System.Decimal"://浮点型  
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理  
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            //写入到客户端
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }


        #endregion

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="extension">Excel文件的扩展名</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string extension, string SheetName, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                if (extension.Equals(".xls") || extension.Equals(".XLS"))
                {
                    workbook = new HSSFWorkbook(file);
                }
                else
                {
                    workbook = new XSSFWorkbook(file);
                }
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="extension">Excel文件的扩展名</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string extension, int SheetIndex, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                if (extension.Equals(".xls") || extension.Equals(".XLS"))
                {
                    workbook = new HSSFWorkbook(file);
                }
                else
                {
                    workbook = new XSSFWorkbook(file);
                }

                string SheetName = workbook.GetSheetName(SheetIndex);
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            string SheetName = workbook.GetSheetName(SheetIndex);
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
        {
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 循环各行各列,写入数据到DataTable
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = null;
                        }
                        else
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dataRow[j] = null;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[j] = cell.ToString();
                                    short format = cell.CellStyle.DataFormat;
                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                    {

                                        DateTime date = cell.DateCellValue;
                                        string re = date.ToString("yyy-MM-dd");
                                        dataRow[j] = re;
                                    }
                                    else
                                    {
                                        if (cell.NumericCellValue.ToString2().Contains("E"))
                                        {
                                            dataRow[j] = Extension.ChangeToDecimal(cell.NumericCellValue.ToString2());
                                        }
                                        else
                                        {
                                            dataRow[j] = cell.NumericCellValue;
                                        }
                                    }
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                default:
                                    dataRow[j] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    //dataRow[j] = row.GetCell(j).ToString();
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出错了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                //sheet.Dispose();
                workbook = null;
                sheet = null;
            }
            #region 清除最后的空行
            for (int i = table.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    table.Rows[i].Delete();
                }
            }
            #endregion
            return table;
        }

        public static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();

            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;

                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }

                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }
        //将内存流保存到文件
        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }

        //将内存流保存到文件
        public static FileStream SaveToFile1(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
                return fs;
            }
        }

        //将内存流输出为下载文件
        public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            if (context.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.BinaryWrite(ms.ToArray());
        }

    }
    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }

}