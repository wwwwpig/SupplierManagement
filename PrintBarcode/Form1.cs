namespace PrintBarcode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //示例1：普通A4打印机，50x50mm二维码
            //bool result1 = QRCodePrinter.PrintQRCode(
            //    content: "Item001",
            //    paperSize: "A2",
            //    qrPosition: new Point(100, 100),  // X=100, Y=100 (单位：0.1mm)
            //    qrSize: new Size(500, 500),       // 宽度=50mm, 高度=50mm (单位：0.1mm)
            //    printerDpi: 300
            //);

            //// 示例2：条码打印机，30x30mm标签
            //bool result2 = QRCodePrinter.PrintQRCode(
            //    content: "ABC123456",
            //    paperSize: "50x30",  // 自定义标签大小：50mm x 30mm
            //    qrPosition: new Point(100, 50),   // X=10mm, Y=5mm
            //    qrSize: new Size(300, 300),       // 宽度=30mm, 高度=30mm
            //    printerDpi: 203  // 条码打印机常见DPI
            //);

            //// 示例3：小尺寸标签打印机
            //bool result3 = QRCodePrinter.PrintQRCode(
            //    content: "ITEM-001-2024",
            //    paperSize: "40x20",  // 40mm x 20mm标签
            //    qrPosition: new Point(50, 30),    // X=5mm, Y=3mm
            //    qrSize: new Size(150, 150),       // 15mm x 15mm二维码
            //    printerDpi: 300
            //);
            try
            {
                if (txtBarcodeHeight.Text.Trim() =="" || txtBarcodeWidth.Text.Trim() == "" || txtBarcodeX.Text.Trim() == "" || txtBarcodeY.Text.Trim() == "" || txtContent.Text.Trim()=="")
                {
                    MessageBox.Show("不能有未填写");
                    return;
                }
                bool result = QRCodePrinter.PrintQRCode(
                    content: txtContent.Text.Trim(),
                    paperSize: txtPaperSpec.Text,
                    qrPosition: new Point(int.Parse(txtBarcodeX.Text), int.Parse(txtBarcodeY.Text)),
                    qrSize: new Size(int.Parse(txtBarcodeWidth.Text), int.Parse(txtBarcodeHeight.Text)),
                    printerDpi: 300
                );
                if (result)
                {
                    tssl.Text = "打印成功";
                }
                else
                {
                    tssl.Text = "打印失败";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void LoadConfig()
        {
            txtPaperSpec.Text = AppConfigHelper.GetValue<string>("PaperSpec", "A4");
            txtBarcodeWidth.Text = AppConfigHelper.GetValue<string>("BarcodeWidth", "30");
            txtBarcodeHeight.Text = AppConfigHelper.GetValue<string>("BarcodeHeight", "30");
            txtBarcodeX.Text = AppConfigHelper.GetValue<string>("BarcodeX", "10");
            txtBarcodeY.Text = AppConfigHelper.GetValue<string>("BarcodeY", "10");

        }

        private void SaveConfig()
        {
            AppConfigHelper.SetValue<string>("PaperSpec", txtPaperSpec.Text);
            AppConfigHelper.SetValue<string>("BarcodeWidth", txtBarcodeWidth.Text);
            AppConfigHelper.SetValue<string>("BarcodeHeight", txtBarcodeHeight.Text);
            AppConfigHelper.SetValue<string>("BarcodeX", txtBarcodeX.Text);
            AppConfigHelper.SetValue<string>("BarcodeY", txtBarcodeY.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConfig();
                tssl.Text = "保存成功";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
