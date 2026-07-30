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
    /// UserPermissionUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class RolePermissionUserControl : UserControl
    {
        PermissionManager permissionManager = new PermissionManager();
        RoleManager roleManager = new RoleManager();
        RolePermissionManager rolePermissionManager = new RolePermissionManager();
        public RolePermissionUserControl()
        {
            InitializeComponent();
        }
        private void QueryRole()
        {
            List<Role> roles = roleManager.GetList();
            lsRole.ItemsSource = roles;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Permission> permissions = permissionManager.GetList().OrderBy(it=>it.Id).ToList();
                lsPermission.ItemsSource = permissions;

                QueryRole();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            
        }

        private bool HasRepetition()
        {
            if (roleManager.HasRole(txtRoleName.Text.Trim()))
            {
                return true;
            }
            return false;
        }
        private bool CheckInput()
        {
            if (txtRoleName.Text.Trim() == "")
            {
                return false;
            }

            return true;
        }

        private void ClearInfo()
        {
            txtRoleName.Text = "";
        }
        private bool AddData()
        {
            bool flag = false;

            Role role = new Role();
            role.RoleName = txtRoleName.Text.Trim();
            role.CreateDt = Utils.GetCurrentDateTime();

            if (roleManager.Insert(role))
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
                    MessageBox.Show("角色已存在");
                    Utils.SetStatus(this, "角色已存在");
                    return;
                }
                if (AddData())
                {
                    Utils.SetStatus(this, "角色添加成功");
                    QueryRole();
                    ClearInfo();
                }
                else
                {
                    MessageBox.Show("角色添加失败。");
                    Utils.SetStatus(this, "角色添加失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private bool DeleteData()
        {
            bool flag = false;
            Role role=lsRole.SelectedItem as Role;
            roleManager.DeleteRole(role);
            flag = true;
            return flag;
        }
        private void btnDele_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DeleteData())
                {
                    QueryRole();
                    Utils.SetStatus(this, "角色删除成功。");
                }
                else
                {
                    MessageBox.Show("角色删除失败。");
                    Utils.SetStatus(this, "角色删除失败。");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private List<Permission> GetCheckedPermissions()
        {
            return lsPermission.SelectedItems.Cast<Permission>().ToList();
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
            List<Permission> checkedPermissions = GetCheckedPermissions();
            return rolePermissionManager.SaveData(role, checkedPermissions);

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveData())
                {
                    Utils.SetStatus(this, "保存成功");
                    //Query();
                    ClearInfo();
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

        private void lsRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var role = lsRole.SelectedItem as Role;
            // 没选角色则清空权限选择
            if (role == null)
            {
                lsPermission.SelectedItems.Clear();
                return;
            }

            //用HashSet是为了后面使用HashSet.Contains(): O(1) - 哈希表查找，几乎瞬间完成,有性能优势
            var permissionIDs = new HashSet<int>(rolePermissionManager.GetPermissionIDsByRole(role));

            // 使用 SelectedItems 同步 ListBoxItem.IsSelected（你的 CheckBox 绑定的是 ListBoxItem.IsSelected）
            lsPermission.SelectedItems.Clear();
            foreach (var item in lsPermission.Items)
            {
                if (item is Permission p && permissionIDs.Contains(p.Id))
                {
                    lsPermission.SelectedItems.Add(p);
                }
            }


        }
    }
}
