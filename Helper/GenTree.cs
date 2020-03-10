using GyIMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace GyIMS.Helper
{
    public static class GenTree
    {
  


        #region  动态获取Tree



        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable();

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T obj in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(obj, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }



        #endregion 
    }
}