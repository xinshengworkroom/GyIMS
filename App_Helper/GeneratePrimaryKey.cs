using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GyIMS.App_Helper
{
    public static class GeneratePrimaryKey
    {
        public static string GetrimaryKey(string _biztype)
        {
            string result = String.Empty;
            using (DBContext db = new DBContext())
            {
                SqlParameter p_BizType = new SqlParameter("@P_BizType", _biztype);
                SqlParameter p_PrimaryKey = new SqlParameter("@P_PrimaryKey", SqlDbType.VarChar, 50);
                p_PrimaryKey.Direction = ParameterDirection.Output;
                db.Database.ExecuteSqlCommand("exec  GetNextPrimaryKey @P_BizType,@P_PrimaryKey out", p_BizType, p_PrimaryKey);
                result = p_PrimaryKey.Value.ToString();
            }
            return result;
        }
    }
}