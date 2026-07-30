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
using WebFirst.Services;

namespace SupplierManagement.Views
{
    /// <summary>
    /// UpdatePwdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePwdWindow : Window
    {
        UserManager userManager = new UserManager();
        public UpdatePwdWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(pbCurrentPwd.Password == "" || pbNewPwd.Password == "" || pbNewPwdAgain.Password == "")
                {
                    MessageBox.Show("请填写完整。");
                    return;
                }
                if (PasswordCryptoHelper.HashPassword(pbCurrentPwd.Password.Trim()) != Utils.user.Pwd)
                {
                    MessageBox.Show("当前密码输入错误。");
                    return;
                }
                if (pbNewPwd.Password != pbNewPwdAgain.Password)
                {
                    MessageBox.Show("两次输入的密码不一致。");
                    return;
                }
                bool flag=userManager.UpdatePwd(Utils.user, PasswordCryptoHelper.HashPassword(pbNewPwd.Password.Trim()));
                if (flag)
                {
                    MessageBox.Show("密码修改成功。");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码修改失败。");
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }
        }
    }
}
