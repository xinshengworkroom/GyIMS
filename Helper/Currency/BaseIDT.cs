using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Helper.Currency
{
    internal class BaseIDT
    {
        private IBaseIDT _IBaseIDT;
        public void SetCon(IBaseIDT IBaseIDT)
        {
            this._IBaseIDT = IBaseIDT;
        }
        public void Connstring(MaintenanceFee MaintenanceFee)
        {
            _IBaseIDT.Connstring(MaintenanceFee);
        }

        public void Connstring(MaintenanceFee MaintenanceFee, MaintenanceFee UpdateColumns)
        {
            _IBaseIDT.Connstring(MaintenanceFee, UpdateColumns);
        }

        //public void ConnString(MaintenanceLog maintenanceLog)
        //{
        //    _IBaseIDT.ConnString(maintenanceLog);
        //}

        public void Query()
        {
            _IBaseIDT.Query();
        }

        public void Add()
        {
            _IBaseIDT.Add();
        }

        public void Update()
        {
            _IBaseIDT.Update();
        }

    }
}