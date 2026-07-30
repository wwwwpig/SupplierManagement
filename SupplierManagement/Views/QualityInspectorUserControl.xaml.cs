using SupplierManagement.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement.Views
{
    /// <summary>
    /// QualityInspectorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class QualityInspectorUserControl : UserControl
    {
        MaterialManager materialManager = new MaterialManager();
        private Material currentMaterial;
        private List<Material> materialsQuery;
        public QualityInspectorUserControl()
        {
            InitializeComponent();
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
                    if (material.IsPass == "合格")
                    {
                        cmbIsPass.SelectedIndex = 0;
                    }
                    else if (material.IsPass == "不合格")
                    {
                        cmbIsPass.SelectedIndex = 1;
                    }
                    else
                    {
                        cmbIsPass.SelectedIndex = -1;
                    }
                    txtReasonForNotPass.Text = material.ReasonForNotPass;
                    txtProcessMode.Text = material.ProcessMode;
                    dtpReturnSupplierTime.SelectedDate = material.ReturnSupplierTime;
                    txtQualityInspectorRemark.Text = material.QualityInspectorRemark;

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
        private bool UpdateData()
        {
            bool flag = false;

            Material material = currentMaterial;

            material.QualityInspector = Utils.user.UserName;
            material.QualityInspectionTime = Utils.GetCurrentDateTime();
            material.IsPass = cmbIsPass.Text;
            material.ReasonForNotPass = txtReasonForNotPass.Text.Trim();
            material.ProcessMode = txtProcessMode.Text.Trim();

            if (dtpReturnSupplierTime.Text != "" && dtpReturnSupplierTime.SelectedDate != null)
            {
                material.ReturnSupplierTime = dtpReturnSupplierTime.SelectedDate.Value;
            }
            material.QualityInspectorRemark = txtQualityInspectorRemark.Text.Trim();

            if (materialManager.Update(material))
            {
                flag = true;
            }
            return flag;


        }
        private void ClearInfo()
        {
            txtMaterialID.Text = "";
            cmbIsPass.SelectedIndex = -1;
            txtReasonForNotPass.Text = "";
            txtProcessMode.Text = "";
            dtpReturnSupplierTime.SelectedDate = null;
            txtQualityInspectorRemark.Text = "";

            currentMaterial = null;
        }
        private bool CheckInput()
        {
            if (cmbIsPass.Text.Trim() == "")
            {
                return false;
            }

            return true;
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
                if (String.IsNullOrEmpty(currentMaterial.Buyer))
                {
                    MessageBox.Show("该物料编码暂无采购员信息，请勿处理。");
                    Utils.SetStatus(this, "该物料编码暂无采购员信息，请勿处理。");
                    return;
                }
                if (!CheckInput())
                {
                    MessageBox.Show("请填写带有*的必填项。");
                    Utils.SetStatus(this, "请填写带有*的必填项");
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

        private void txtMaterialID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (e.Key==Key.Enter)
            //{
            //    try
            //    {
            //        if (materialsQuery == null)
            //        {
            //            MessageBox.Show("请先执行查询，再扫物料条码。");
            //            txtMaterialID.Text = "";
            //            return;
            //        }
            //        List<Material> materials = materialsQuery.Where(it =>it.MaterialID == txtMaterialID.Text.Trim()).ToList();
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

        private void cmbIsPass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = cmbIsPass.SelectedIndex;

                if (index == 0)
                {
                    // 合格
                    txtReasonForNotPass.IsEnabled = false;
                    txtProcessMode.IsEnabled = false;
                    dtpReturnSupplierTime.IsEnabled = false;
                }
                else if (index == 1)
                {
                    // 不合格
                    txtReasonForNotPass.IsEnabled = true;
                    txtProcessMode.IsEnabled = true;
                    dtpReturnSupplierTime.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
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
