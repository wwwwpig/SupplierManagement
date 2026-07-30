using SupplierManagement.Common;
using System.Windows;
using System.Windows.Controls;
using WebFirst.Entities;
using WebFirst.Services;

namespace SupplierManagement.Views
{
    /// <summary>
    /// BuyUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class BuyUserControl : UserControl
    {
        MaterialManager materialManager = new MaterialManager();
        SupplierManager supplierManager = new SupplierManager();

        private Material currentMaterial;
        public BuyUserControl()
        {
            InitializeComponent();
        }
        private void Query()
        {
            List<Material> materials = materialManager.GetMaterialsByQuery(txtProjectIdQuery.Text.Trim(), txtProjectNameQuery.Text.Trim(), txtDeviceNameQuery.Text.Trim(), txtMaterialIDQuery.Text.Trim(), txtMaterialNameQuery.Text.Trim(),cmbSupplierQuery.Text);
            dg.ItemsSource = materials;
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
                    cmbSupplierName.Text = material.SupplierName;
                    txtBuyerRemark.Text = material.BuyerRemark;
                    dpBuyDate.SelectedDate = material.BuyTime;
                    dpPlannedDeliveryDate.SelectedDate = material.PlannedDeliveryDate;

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

            material.SupplierName = cmbSupplierName.Text.Trim();
            material.Buyer = Utils.user.UserName;
            material.BuyerRemark = txtBuyerRemark.Text.Trim();
            if (dpBuyDate.Text != "" && dpBuyDate.SelectedDate != null)
            {
                material.BuyTime = dpBuyDate.SelectedDate.Value;
            }
            if (dpPlannedDeliveryDate.Text!="" && dpPlannedDeliveryDate.SelectedDate != null)
            {
                material.PlannedDeliveryDate = dpPlannedDeliveryDate.SelectedDate.Value;
            }
            
            if (materialManager.Update(material))
            {
                flag = true;
            }
            return flag;


        }
        private void ClearInfo()
        {
            cmbSupplierName.SelectedIndex = -1;
            txtBuyerRemark.Text = "";
            txtMaterialID.Text = "";
            dpBuyDate.SelectedDate = null;
            dpPlannedDeliveryDate.SelectedDate = null;
            currentMaterial = null;
        }
        private bool CheckInput()
        {
            if (String.IsNullOrEmpty(cmbSupplierName.Text))
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
                if(currentMaterial == null)
                {
                    MessageBox.Show("请选择要修改的物料编码。");
                    Utils.SetStatus(this, "请选择要修改的物料编码。");
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                SupplierManager supplierManager = new SupplierManager();
                List<string> supplierNames = supplierManager.GetAllSuppliers().Select(s => s.SupplierName).ToList();
                supplierNames.Insert(0, "全部");
                cmbSupplierQuery.ItemsSource = supplierNames;
                cmbSupplierQuery.SelectedIndex = 0;


                List<Supplier> suppliers = supplierManager.GetAllSuppliers();
                cmbSupplierName.ItemsSource = suppliers;
                cmbSupplierName.DisplayMemberPath = "SupplierName";
                cmbSupplierName.SelectedValuePath = "SupplierName";
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Utils.SetStatus(this, "窗体加载失败，请查看日志。");
            }
        }
    }
}
