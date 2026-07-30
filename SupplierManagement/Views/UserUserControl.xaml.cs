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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement.Views
{
    /// <summary>
    /// UserUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserUserControl : UserControl
    {
        UserManager userManager = new UserManager();
        public UserUserControl()
        {
            InitializeComponent();
        }
        private void Query()
        {
            List<User> users=userManager.GetUsersByQuery(txtUserIDQuery.Text.Trim(), txtUserNameQuery.Text.Trim());
            dg.ItemsSource = users;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Query();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Utils.SetStatus(this, "查询失败。");
            }
        }
        private bool HasRepetition()
        {
            if (userManager.HasUser(txtUserID.Text.Trim()))
            {
                return true;
            }
            return false;
        }
        private bool CheckInput()
        {
            if (txtUserID.Text.Trim() == "" || txtUserName.Text.Trim() == "" || pwd.Password == "")
            {
                return false;
            }

            return true;
        }

        private void ClearInfo()
        {
            txtUserID.Text = "";
            txtUserName.Text = "";
            pwd.Password = "";
            txtRemark.Text = "";
        }
        private bool AddData()
        {
            bool flag = false;

            User user = new User();
            user.UserID = txtUserID.Text.Trim();
            user.UserName = txtUserName.Text.Trim();
            user.Pwd = PasswordCryptoHelper.HashPassword(pwd.Password.Trim());
            user.Remark = txtRemark.Text.Trim();
            user.CreateDT = Utils.GetCurrentDateTime();

            if (userManager.Insert(user))
            {
                flag = true;
            }
            return flag;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckInput())
                {
                    MessageBox.Show("请填写带有*的必填项");
                    Utils.SetStatus(this, "请填写带有*的必填项");
                    return;
                }
                if (HasRepetition())
                {
                    MessageBox.Show("用户名已存在");
                    Utils.SetStatus(this, "用户名已存在");
                    return;
                }
                if (AddData())
                {
                    Utils.SetStatus(this, "用户添加成功");
                    Query();
                    ClearInfo();
                }
                else
                {
                    MessageBox.Show("用户添加失败。");
                    Utils.SetStatus(this, "用户添加失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void SetAllCheckBoxes(bool isChecked)
        {
            // 遍历DataGrid的所有数据项
            foreach (var item in dg.Items)
            {
                // 获取对应的DataGridRow容器
                var row = dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (row != null)
                {
                    // 获取第一列的单元格内容并转换为CheckBox
                    var checkBox = dg.Columns[0].GetCellContent(row) as CheckBox;

                    if (checkBox != null)
                    {
                        checkBox.IsChecked = isChecked;
                    }
                }
            }
        }
        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkAll.IsChecked == true)
                {
                    SetAllCheckBoxes(true);
                }
                else
                {
                    SetAllCheckBoxes(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        // 获取所有选中行的数据对象集合
        public List<User> GetCheckedRows()
        {
            var checkedItems = new List<User>();

            foreach (User item in dg.Items)
            {
                // 获取DataGridRow容器
                var row = dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (row != null)
                {
                    // 获取第一列的单元格（DataGridCheckBoxColumn通常在第0列）
                    var cell = dg.Columns[0].GetCellContent(row) as CheckBox;

                    if (cell != null && cell.IsChecked == true)
                    {
                        // 将选中的数据对象加入集合
                        checkedItems.Add(item);
                    }
                }
            }

            return checkedItems;
        }
        private bool DeleteData()
        {
            bool flag = false;
            List<User> checkedItems = GetCheckedRows();
            userManager.DeleteUser(checkedItems);
            flag = true;
            return flag;
        }
        private bool HasCheckedRows()
        {
            return GetCheckedRows().Count > 0;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!HasCheckedRows())
                {
                    MessageBox.Show("请选择要删除的用户。");
                    Utils.SetStatus(this, "请选择要删除的用户。");
                    return;
                }
                if (DeleteData())
                {
                    Query();
                    Utils.SetStatus(this, "删除用户成功。");
                    chkAll.IsChecked = false;
                }
                else
                {
                    MessageBox.Show("删除用户失败。");
                    Utils.SetStatus(this, "删除用户失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private bool ResetPwd()
        {
            List<User> checkedItems = GetCheckedRows();
            string newPwd = PasswordCryptoHelper.HashPassword("000000");
            return userManager.ResetPwd(checkedItems, newPwd);
        }

        private void btnResetPwd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!HasCheckedRows())
                {
                    MessageBox.Show("请选择要重置密码的用户。");
                    Utils.SetStatus(this, "请选择要重置密码的用户。");
                    return;
                }
                if (ResetPwd())
                {
                    Utils.SetStatus(this, "密码重置成功");
                    ClearInfo();
                }
                else
                {
                    MessageBox.Show("密码重置失败。");
                    Utils.SetStatus(this, "密码重置失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
