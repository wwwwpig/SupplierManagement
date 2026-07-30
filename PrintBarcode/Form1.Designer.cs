namespace PrintBarcode
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnPrint = new Button();
            label1 = new Label();
            txtPaperSpec = new TextBox();
            label2 = new Label();
            txtBarcodeWidth = new TextBox();
            label3 = new Label();
            txtBarcodeHeight = new TextBox();
            label4 = new Label();
            txtBarcodeX = new TextBox();
            label5 = new Label();
            txtBarcodeY = new TextBox();
            btnSave = new Button();
            label6 = new Label();
            txtContent = new TextBox();
            statusStrip1 = new StatusStrip();
            tssl = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnPrint
            // 
            btnPrint.Location = new Point(426, 346);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(170, 62);
            btnPrint.TabIndex = 0;
            btnPrint.Text = "打印";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 52);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 1;
            label1.Text = "纸张规格";
            // 
            // txtPaperSpec
            // 
            txtPaperSpec.Location = new Point(146, 52);
            txtPaperSpec.Name = "txtPaperSpec";
            txtPaperSpec.Size = new Size(213, 30);
            txtPaperSpec.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 127);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 1;
            label2.Text = "条码宽度";
            // 
            // txtBarcodeWidth
            // 
            txtBarcodeWidth.Location = new Point(146, 126);
            txtBarcodeWidth.Name = "txtBarcodeWidth";
            txtBarcodeWidth.Size = new Size(213, 30);
            txtBarcodeWidth.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(390, 127);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 1;
            label3.Text = "条码高度";
            // 
            // txtBarcodeHeight
            // 
            txtBarcodeHeight.Location = new Point(488, 122);
            txtBarcodeHeight.Name = "txtBarcodeHeight";
            txtBarcodeHeight.Size = new Size(213, 30);
            txtBarcodeHeight.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(36, 202);
            label4.Name = "label4";
            label4.Size = new Size(94, 24);
            label4.TabIndex = 1;
            label4.Text = "条码X坐标";
            // 
            // txtBarcodeX
            // 
            txtBarcodeX.Location = new Point(146, 202);
            txtBarcodeX.Name = "txtBarcodeX";
            txtBarcodeX.Size = new Size(213, 30);
            txtBarcodeX.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(379, 202);
            label5.Name = "label5";
            label5.Size = new Size(93, 24);
            label5.TabIndex = 1;
            label5.Text = "条码Y坐标";
            // 
            // txtBarcodeY
            // 
            txtBarcodeY.Location = new Point(488, 202);
            txtBarcodeY.Name = "txtBarcodeY";
            txtBarcodeY.Size = new Size(213, 30);
            txtBarcodeY.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(180, 346);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(170, 62);
            btnSave.TabIndex = 3;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(48, 277);
            label6.Name = "label6";
            label6.Size = new Size(82, 24);
            label6.TabIndex = 1;
            label6.Text = "条码内容";
            // 
            // txtContent
            // 
            txtContent.Location = new Point(146, 274);
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(555, 30);
            txtContent.TabIndex = 4;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tssl });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // tssl
            // 
            tssl.Name = "tssl";
            tssl.Size = new Size(0, 15);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(txtContent);
            Controls.Add(btnSave);
            Controls.Add(txtBarcodeY);
            Controls.Add(label5);
            Controls.Add(txtBarcodeHeight);
            Controls.Add(txtBarcodeX);
            Controls.Add(label3);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(txtBarcodeWidth);
            Controls.Add(label2);
            Controls.Add(txtPaperSpec);
            Controls.Add(label1);
            Controls.Add(btnPrint);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "条码打印工具";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPrint;
        private Label label1;
        private TextBox txtPaperSpec;
        private Label label2;
        private TextBox txtBarcodeWidth;
        private Label label3;
        private TextBox txtBarcodeHeight;
        private Label label4;
        private TextBox txtBarcodeX;
        private Label label5;
        private TextBox txtBarcodeY;
        private Button btnSave;
        private Label label6;
        private TextBox txtContent;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tssl;
    }
}
