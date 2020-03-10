using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Helper.Currency
{
    internal interface  IBaseIDT
    {
        void Connstring(MaintenanceFee MaintenanceFee);

        void Connstring(MaintenanceFee MaintenanceFee, MaintenanceFee UpdateColumns);
 
        void Query();

        void Add();

        void Update();

        //void ConnString(MaintenanceLog maintenanceLog);

    }
}