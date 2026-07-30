using Microsoft.Win32;
using NPOI.HPSF;
using NPOI.Util;
using SupplierManagement.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// DesignUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DesignUserControl : UserControl
    {
        MaterialManager materialManager = new MaterialManager();
        private Material currentMaterial;
        public DesignUserControl()
        {
            InitializeComponent();
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

        private void Query()
        {
            List<Material> materials = materialManager.GetMaterialsByQuery(txtProjectIdQuery.Text.Trim(), txtProjectNameQuery.Text.Trim(), txtDeviceNameQuery.Text.Trim(), txtMaterialIDQuery.Text.Trim(), txtMaterialNameQuery.Text.Trim(), cmbSupplierQuery.Text);
            dg.ItemsSource = materials;
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dg.SelectedItem is Material material)
                {
                    txtMaterialIDBefore.Text = material.MaterialID;
                    currentMaterial = material;
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
                Utils.SetStatus(this, "系统异常，请查看日志。");
            }
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog=new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx;"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    #region 查重复
                    List<Material> existingMaterials = materialManager.GetAllMaterials();
                    bool isPass = ExcelHelper.CheckExcelComponents(filePath, existingMaterials);
                    if (!isPass)
                    {
                        MessageBox.Show("Excel不符合导入条件。请检查Excel：红色ID重复记录，黄色为必填项。");
                        Utils.SetStatus(this, "Excel不符合导入条件。请检查Excel：红色ID重复记录，黄色为必填项。");
                        return;
                    }
                    #endregion

                    List<Material> materials = Common.ExcelHelper.LoadMaterialFromExcel(filePath);
                    if (materialManager.InsertRange(materials))
                    {
                        Utils.SetStatus(this, "导入成功");
                    }
                    else
                    {
                        MessageBox.Show("导入失败");
                        Utils.SetStatus(this, "导入失败");
                    }
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

        private void btnChangeMaterialID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMaterialIDAfter.Text.Trim() != "")
                {
                    Material newMaterial = new Material();
                    newMaterial = currentMaterial.Copy<Material>();

                    newMaterial.ChangeHistory= currentMaterial.ChangeHistory+$"{Utils.user.UserName}({Utils.user.UserID})在{Utils.GetCurrentDateTime().ToString("yyyy-MM-dd HH:mm:ss")}将物料编码由{txtMaterialIDBefore.Text.Trim()}变更为{txtMaterialIDAfter.Text.Trim()}\n------------------------\n";
                    newMaterial.MaterialID = txtMaterialIDAfter.Text.Trim();

                    currentMaterial.Status = "已作废";

                    MaterialManager.Db.BeginTran();

                    materialManager.Update(currentMaterial);
                    materialManager.Insert(newMaterial);

                    MaterialManager.Db.CommitTran();

                    currentMaterial = null;

                    Query();

                }
            }
            catch (Exception ex)
            {
                MaterialManager.Db.RollbackTran();
                Log.Error(ex);
                Utils.SetStatus(this, "系统异常，请查看日志。");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SupplierManager supplierManager = new SupplierManager();
                List<string> supplierNames = supplierManager.GetAllSuppliers().Select(s=>s.SupplierName).ToList();
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
