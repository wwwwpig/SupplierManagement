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
using WebFirst.Services;
using WebFirst.Entities;

namespace SupplierManagement.Views
{
    /// <summary>
    /// InWarehouseUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class InWarehouseUserControl : UserControl
    {
        MaterialManager materialManager = new MaterialManager();
        private Material currentMaterial;
        private List<Material> materialsQuery;
        public InWarehouseUserControl()
        {
            InitializeComponent();
        }
        private bool UpdateData()
        {
            bool flag = false;

            Material material = currentMaterial;

            material.PersonIn = Utils.user.UserName;
            material.InWarehouseTime = Utils.GetCurrentDateTime();
            material.PersonInRemark = txtPersonInRemark.Text.Trim();

            if (materialManager.Update(material))
            {
                flag = true;
            }
            return flag;


        }
        private void ClearInfo()
        {
            txtMaterialID.Text = "";
            txtPersonInRemark.Text = "";

            currentMaterial = null;
        }

        /// <summary>
        /// 是否已经作废了
        /// </summary>
        /// <returns>true：已作废 false：正常</returns>
        private bool IsCancellation()
        {
            if (currentMaterial.Status == "已作废")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentMaterial == null)
                {
                    MessageBox.Show("请先扫描物料编码。");
                    Utils.SetStatus(this, "请先扫描物料编码。");
                    return;
                }
                if (String.IsNullOrEmpty(currentMaterial.QualityInspector))
                {
                    MessageBox.Show("该物料编码暂无质检员信息，请勿处理。");
                    Utils.SetStatus(this, "该物料编码暂无质检员信息，请勿处理。");
                    return;
                }
                if (IsCancellation())
                {
                    MessageBox.Show("该物料编码已作废，请勿操作。");
                    Utils.SetStatus(this, "该物料编码已作废，请勿操作。");
                    return;
                }
                if (UpdateData())
                {
                    Utils.SetStatus(this, "保存成功");
                    Query();
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

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dg.SelectedItem is Material material)
                {
                    currentMaterial = material;

                    txtMaterialID.Text = material.MaterialID;
                    txtPersonInRemark.Text=material.PersonInRemark;

                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                Utils.SetStatus(this, "系统异常，请查看日志。");
            }
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

        private void txtMaterialID_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    try
            //    {
            //        if (materialsQuery == null)
            //        {
            //            MessageBox.Show("请先执行查询，再扫物料条码。");
            //            txtMaterialID.Text = "";
            //            return;
            //        }
            //        List<Material> materials = materialsQuery.Where(it => it.MaterialID == txtMaterialID.Text.Trim()).ToList();
            //        if (materials.Count > 0)
            //        {
            //            dg.ItemsSource = materials;
            //            if (materials.Count == 1)
            //            {
            //                currentMaterial = materials[0];
            //                dgOperate.DataContext = currentMaterial;
            //                btnSave.IsEnabled = true;
            //            }
            //            else
            //            {
            //                MessageBox.Show("该物料编码有多条记录，请核查。");
            //                Utils.SetStatus(this, "该物料编码有多条记录，请核查。");
            //                btnSave.IsEnabled = false;
            //            }

            //        }
            //        else
            //        {
            //            MessageBox.Show("未找到该物料编码。");
            //            Utils.SetStatus(this, "未找到该物料编码。");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex);
            //    }
            //}
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
