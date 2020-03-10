using GyIMS.Helper.Container;
using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Helper.Currency
{
    internal class IDTMaintenanceFees : IBaseIDT
    {
        private IMaintenanceFeeDal _IMaintenanceFeeDal = BaseContainer.Resolve<IMaintenanceFeeDal, MaintenanceFeeDal>();


        MaintenanceFee _MaintenanceFee = new MaintenanceFee();
        MaintenanceFee _UpdateMaintenanceFee = new MaintenanceFee();
        public void Connstring(MaintenanceFee MaintenanceFee)
        {
            _MaintenanceFee = MaintenanceFee;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="MaintenanceFee"></param>
        /// <param name="UpdateColumns"></param>
        public void Connstring(MaintenanceFee MaintenanceFee, MaintenanceFee UpdateColumns)
        {
            _MaintenanceFee.ID = MaintenanceFee.ID;
            _UpdateMaintenanceFee = UpdateColumns;
            _IMaintenanceFeeDal.GetModels(u => u.ID == _MaintenanceFee.ID).ToList();
            
        }

        public void Query()
        {
            _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == _MaintenanceFee.MaintenanceID).ToList();
            
        }

        public void Add()
        {
            _IMaintenanceFeeDal.Add(_MaintenanceFee);
        }

        public void Update()
        {
            _IMaintenanceFeeDal.Update(_MaintenanceFee);
        }

        
    }
}