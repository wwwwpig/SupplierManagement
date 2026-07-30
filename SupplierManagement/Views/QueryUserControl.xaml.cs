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
    /// QueryUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryUserControl : UserControl
    {
        MaterialManager materialManager = new MaterialManager();
        //private Material currentMaterial;
        private List<Material> materialsQuery;

        public QueryUserControl()
        {
            InitializeComponent();
        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                // 设置行号为行索引+1（从1开始）
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                Utils.SetStatus(this, "系统异常，请查看日志。");
            }
        }
        private void Query()
        {
            materialsQuery = materialManager.GetMaterialsByQuery(txtProjectIdQuery.Text.Trim(), txtProjectNameQuery.Text.Trim(), txtDeviceNameQuery.Text.Trim(), txtMaterialIDQuery.Text.Trim(), txtMaterialNameQuery.Text.Trim(), cmbSupplierQuery.Text);
            dg.ItemsSource = materialsQuery;
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
                Utils.SetStatus(this, "系统异常，请查看日志。");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SupplierManager supplierManager = new SupplierManager();
                List<string> supplierNames = supplierManager.GetAllSuppliers().Select(s => s.SupplierName).ToList();
                supplierNames.Insert(0, "全部");
                cmbSupplierQuery.ItemsSource = supplierNames;
                cmbSupplierQuery.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Utils.SetStatus(this, "窗体加载失败，请查看日志。");
            }
        }
    }
}
