using SupplierManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserManager userManager = new UserManager();

                if (txtUserID.Text.Trim() == "" || pwdPassword.Password.Trim() == "")
                {
                    lblMessage.Text = "请填写用户名和密码";
                    lblMessage.Visibility = Visibility.Visible;
                }

                User user = userManager.GetFirst(it=>it.UserID == txtUserID.Text.Trim());
                if (user == null)
                {
                    lblMessage.Text = "用户不存在";
                    lblMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    if (PasswordCryptoHelper.VerifyPassword(pwdPassword.Password.Trim(), user.Pwd))
                    {
                        Utils.user = user;
                        //App.Current.Properties["User"] = user;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        

                        this.Close();
                    }
                    else
                    {
                        lblMessage.Text = "密码错误";
                        lblMessage.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "系统异常，请查看日志。";
                Log.Error(ex);
            }
        }
    }
}
