using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintBarcode
{
    public class QRCodePrinter
    {
        /// <summary>
        /// 生成并打印二维码（支持条码打印机）
        /// </summary>
        /// <param name="content">要生成二维码的字符串内容</param>
        /// <param name="paperSize">纸张大小，如"A4"、"Letter"，或自定义大小如"100x150"</param>
        /// <param name="qrPosition">二维码在纸张上的位置（X, Y坐标）</param>
        /// <param name="qrSize">二维码的大小（宽度和高度）</param>
        /// <param name="printerDpi">打印机DPI（条码打印机通常为203或300）</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool PrintQRCode(
            string content,
            string paperSize,
            Point qrPosition,
            Size qrSize,
            int printerDpi = 300)
        {
            try
            {
                // 验证输入参数
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine("错误：二维码内容不能为空");
                    return false;
                }

                if (qrSize.Width <= 0 || qrSize.Height <= 0)
                {
                    Console.WriteLine("错误：二维码大小必须大于0");
                    return false;
                }

                // 生成二维码图像
                Bitmap qrCodeImage = GenerateQRCode(content, qrSize, printerDpi);
                if (qrCodeImage == null)
                {
                    Console.WriteLine("错误：二维码生成失败");
                    return false;
                }

                // 执行打印
                return PrintImage(qrCodeImage, paperSize, qrPosition, qrSize, printerDpi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打印二维码时发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 生成二维码图像（支持高DPI打印）
        /// </summary>
        private static Bitmap GenerateQRCode(string content, Size size, int dpi)
        {
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                        content,
                        QRCodeGenerator.ECCLevel.H // 条码打印机建议使用H级（最高容错率）
                    );

                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        // 计算像素大小
                        int pixelSize = Math.Max(size.Width, size.Height) * dpi / 25; // mm to pixels (约等于)

                        Bitmap bitmap = qrCode.GetGraphic(
                            pixelsPerModule: Math.Max(2, pixelSize / 30), // 根据大小自动调整模块大小
                            darkColor: Color.Black,
                            lightColor: Color.White,
                            drawQuietZones: true
                        );

                        // 设置图像DPI
                        bitmap.SetResolution(dpi, dpi);

                        return bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"二维码生成错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 打印图像到指定纸张和位置
        /// </summary>
        private static bool PrintImage(Image image, string paperSize, Point position, Size size, int dpi)
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();

                // 设置打印机
                printDoc.PrinterSettings.PrinterName = GetDefaultPrinter();

                // 处理条码打印机的特殊纸张设置
                if (IsLabelPrinter(printDoc.PrinterSettings.PrinterName))
                {
                    SetupLabelPrinter(printDoc, paperSize, dpi);
                }
                else
                {
                    // 标准打印机纸张设置
                    PaperSize selectedPaperSize = GetPaperSize(printDoc.PrinterSettings, paperSize);
                    if (selectedPaperSize != null)
                    {
                        printDoc.DefaultPageSettings.PaperSize = selectedPaperSize;
                    }
                }

                // 设置高质量打印模式（对条码打印机很重要）
                printDoc.DefaultPageSettings.PrinterResolution = new PrinterResolution
                {
                    X = dpi,
                    Y = dpi
                };

                // 打印事件处理
                printDoc.PrintPage += (sender, e) =>
                {
                    // 使用高质量渲染
                    e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    // 在指定位置绘制二维码
                    e.Graphics.DrawImage(image, new Rectangle(position, size));

                    Console.WriteLine($"已打印二维码: 位置({position.X},{position.Y}), 大小{size.Width}x{size.Height}");
                };

                // 执行打印
                printDoc.Print();

                Console.WriteLine("二维码打印成功！");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打印错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检测是否为条码/标签打印机
        /// </summary>
        private static bool IsLabelPrinter(string printerName)
        {
            string[] labelPrinterKeywords = { "label", "条码", "标签", "zebra", "brother", "tsc" };
            string lowerPrinterName = printerName.ToLower();

            foreach (var keyword in labelPrinterKeywords)
            {
                if (lowerPrinterName.Contains(keyword))
                {
                    Console.WriteLine($"检测到条码打印机: {printerName}");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置条码打印机参数
        /// </summary>
        private static void SetupLabelPrinter(PrintDocument printDoc, string paperSize, int dpi)
        {
            try
            {
                // 解析自定义纸张大小（格式：宽度x高度，单位mm）
                if (paperSize.Contains("x"))
                {
                    var parts = paperSize.Split('x');
                    if (parts.Length == 2 &&
                        float.TryParse(parts[0], out float widthMm) &&
                        float.TryParse(parts[1], out float heightMm))
                    {
                        // 转换为英寸（1英寸=25.4mm）
                        float widthInch = widthMm / 25.4f;
                        float heightInch = heightMm / 25.4f;

                        // 转换为像素
                        int widthPixel = (int)(widthInch * dpi);
                        int heightPixel = (int)(heightInch * dpi);

                        // 创建自定义纸张大小
                        PaperSize customSize = new PaperSize("Custom", widthPixel, heightPixel);
                        printDoc.DefaultPageSettings.PaperSize = customSize;

                        Console.WriteLine($"设置自定义标签大小: {widthMm}mm x {heightMm}mm");
                    }
                }

                // 设置条码打印机特性
                printDoc.DefaultPageSettings.Landscape = false;
                printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"条码打印机设置警告: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取默认打印机名称
        /// </summary>
        private static string GetDefaultPrinter()
        {
            try
            {
                return new PrinterSettings().PrinterName;
            }
            catch
            {
                var printers = PrinterSettings.InstalledPrinters;
                if (printers.Count > 0)
                {
                    return printers[0].ToString();
                }
                throw new Exception("未找到可用的打印机");
            }
        }

        /// <summary>
        /// 根据名称查找纸张大小
        /// </summary>
        private static PaperSize GetPaperSize(PrinterSettings settings, string paperSizeName)
        {
            foreach (PaperSize size in settings.PaperSizes)
            {
                if (size.PaperName.Equals(paperSizeName, StringComparison.OrdinalIgnoreCase))
                {
                    return size;
                }
            }
            return null;
        }
    }
}
