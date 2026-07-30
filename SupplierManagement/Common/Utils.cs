using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement.Common
{
    public class Utils
    {
        #region 用于记录登录的用户信息
        public static User user { get; set;}
        //public static int UserPrimaryKey { get; set; }
        //public static string UserID { get; set; }
        //public static string UserName { get; set; }
        //public static string UserPwd { get; set; }
        //public static int RoleID { get; set; }
        //public static string RoleName { get; set; }
        #endregion

        public static DateTime GetCurrentDateTime()
        {
            return Repository<Material>.Db.GetDate();
        }

        public static void SetStatus(DependencyObject dependencyObject, string text)
        {
            Window parentWindow = Window.GetWindow(dependencyObject);
            if (parentWindow is SupplierManagement.MainWindow mainWindow)
            {
                mainWindow.SetStatus(text);
            }
        }
    }
}
