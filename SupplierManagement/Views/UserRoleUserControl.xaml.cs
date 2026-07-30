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
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement.Views
{
    /// <summary>
    /// UserRoleUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserRoleUserControl : UserControl
    {
        UserRoleManager userRoleManager = new UserRoleManager();
        RoleManager roleManager = new RoleManager();
        public UserRoleUserControl()
        {
            InitializeComponent();
        }
        private List<User> GetCheckedUsers()
        {
            return lsUser.SelectedItems.Cast<User>().ToList();
        }
        private bool SaveData()
        {
            if (lsRole.SelectedItem == null)
            {
                MessageBox.Show("请选择角色");
                Utils.SetStatus(this, "请选择角色");
                return false;
            }
            Role role = lsRole.SelectedItem as Role;
            List<User> checkedUsers = GetCheckedUsers();
            return userRoleManager.SaveData(role, checkedUsers);

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveData())
                {
                    Utils.SetStatus(this, "保存成功");
                    //Query();
                }
                else
                {
                    MessageBox.Show("保存失败。");
                    Utils.SetStatus(this, "保存失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void QueryUser()
        {
            List<User> users = new UserManager().GetList().OrderBy(u=>u.UserName).ToList();
            lsUser.ItemsSource = users;
        }
        private void QueryRole()
        {
            List<Role> roles = roleManager.GetList().OrderBy(r=>r.Id).ToList();
            lsRole.ItemsSource = roles;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                QueryUser();
                QueryRole();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void lsRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var role = lsRole.SelectedItem as Role;
            // 没选角色则清空权限选择
            if (role == null)
            {
                lsUser.SelectedItems.Clear();
                return;
            }

            //用HashSet是为了后面使用HashSet.Contains(): O(1) - 哈希表查找，几乎瞬间完成,有性能优势
            var usersIDs = new HashSet<int>(userRoleManager.GetUserIDsByRole(role));

            // 使用 SelectedItems 同步 ListBoxItem.IsSelected（你的 CheckBox 绑定的是 ListBoxItem.IsSelected）
            lsUser.SelectedItems.Clear();
            foreach (var item in lsUser.Items)
            {
                if (item is User u && usersIDs.Contains(u.Id))
                {
                    lsUser.SelectedItems.Add(u);
                }
            }
        }
    }
}
