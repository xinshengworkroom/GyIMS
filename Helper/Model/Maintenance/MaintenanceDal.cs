using GyIMS.Helper.Container;
using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GyIMS.Helper
{
    public class MaintenanceDal :Query<Maintenance> ,IMaintenanceDal
    {
        public override int Add(Maintenance t)
        {
            int result = base.Add(t);
            if (!string.IsNullOrEmpty(t.FileName))
            {
                IMaintenanceFileDal maintenanceFileDal = new MaintenanceFileDal();
                string[] files = t.FileName.Split(',');
                foreach (string file in files)
                {
                    result += maintenanceFileDal.Add(new MaintenanceFile
                    {
                        MaintenanceID = t.ID,
                        FileName = file,
                        CreateDate = t.CreateDate,
                        CreatePerson = t.CreatePerson,
                        Type = Enums.MaintenanceFileTypeEnum.Able,
                    });
                }
            }
            return result;
        }

        public override int Update(Maintenance t)
        {
            int result = base.Update(t);
            if (!string.IsNullOrEmpty(t.FileName))
            {
                IMaintenanceFileDal maintenanceFileDal = new MaintenanceFileDal();
                result += maintenanceFileDal.ExecuteSql(@"delete from MaintenanceFiles where  MaintenanceID = @MaintenanceID", new SqlParameter[] {
                    new SqlParameter("@MaintenanceID",t.ID)
                });

                string[] files = t.FileName.Split(',');
                foreach (string file in files)
                {
                    result += maintenanceFileDal.Add(new MaintenanceFile
                    {
                        MaintenanceID = t.ID,
                        FileName = file,
                        FilePath = "UploadFile/" + file,
                        CreateDate = t.CreateDate,
                        CreatePerson = t.CreatePerson,
                        Type = Enums.MaintenanceFileTypeEnum.Able,
                    });
                }
            }

            return result;
        }

    }

    
}