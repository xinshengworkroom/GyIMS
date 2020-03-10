using System;
using System.Data.Entity;
using System.Linq;
namespace GyIMS.Models
{
    

    public class DBContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“DBContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“GyIMS.Models.DBContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“DBContext”
        //连接字符串。
        public DBContext()
            : base("name=DBContext")
        {
        }

        public System.Data.Entity.DbSet<GyIMS.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.Menu> Menus { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.Role> Roles { get; set; }

        #region Maps
        public System.Data.Entity.DbSet<GyIMS.Models.MapUserRole> MapsUserRole { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MapManagerUser> MapsManagerUser { get; set; }
        public System.Data.Entity.DbSet<GyIMS.Models.MapRoleMenu> MapsRoleMenu { get; set; }

        #endregion



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MapManagerUser>().ToTable("MapsManagerUser");
            modelBuilder.Entity<MapRoleMenu>().ToTable("MapsRoleMenu");
            modelBuilder.Entity<MapUserRole>().ToTable("MapsUserRole");
            //modelBuilder.Entity<MapsMenuTitle>().ToTable("MapsMenuTitles");
        }

        public System.Data.Entity.DbSet<GyIMS.Models.Contact> Contacts { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.ItRole> ItRoles { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.Maintenance> Maintenances { get; set; }

        


        public System.Data.Entity.DbSet<GyIMS.Models.MaintenanceFile> MaintenanceFiles { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MaintenanceFee> MaintenanceFees { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MaintenanceHandler> MaintenanceHandlers { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MaintenanceResult> MaintenanceResults { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestForm> RequestForms { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormAssessment> RequestFormAssessments { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormPriceList> RequestFormPriceLists { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormTask> RequestFormTasks { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormTaskList> RequestFormTaskLists { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormFile> RequestFormFiles { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.RequestFormBug> RequestFormBugs { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.Message> Messages { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.StatusInfo> StatusInfoes { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MenuTitle> MenuTitles { get; set; }
        public System.Data.Entity.DbSet<GyIMS.Models.MapsMenuTitle> MapsMenuTitles { get; set; }

        public System.Data.Entity.DbSet<GyIMS.Models.MaintenanceLog> MaintenanceLogs { get; set; }




        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。



    }


}