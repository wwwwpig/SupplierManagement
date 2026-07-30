using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFirst.Entities;


namespace SupplierManagement.Common
{
    public class ExcelHelper
    {
        public static List<Material> LoadMaterialFromExcel(string filePath)
        {
            var materials = new List<Material>();
            var createDt = Utils.GetCurrentDateTime();
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0); // 默认读取第一个Sheet

                IRow row0=sheet.GetRow(0);
 
                string projectID = row0.GetCell(1).ToString();
                string projectName = row0.GetCell(3).ToString();
                string deviceName = row0.GetCell(5).ToString();
    


                for(int i=3;i<=sheet.LastRowNum;i++)
                {
                    IRow row=sheet.GetRow(i);
                    if (row==null) continue;

                    var material = new Material
                    {
                        MaterialID = row.GetCell(2).ToString(),
                        ProjectID = projectID,
                        ProjectName = projectName,
                        DeviceName = deviceName,
                        BOMLevel = row.GetCell(1).ToString(),
                        MaterialName = row.GetCell(3).ToString(),
                        Qty = Convert.ToDecimal(row.GetCell(4).ToString()),
                        Unit = row.GetCell(5)?.ToString() ?? "",
                        Spec = row.GetCell(6)?.ToString() ?? "",
                        MaterialRole = row.GetCell(7)?.ToString() ?? "",
                        DesignStatus = row.GetCell(8)?.ToString() ?? "",
                        WorkmanShip = row.GetCell(9)?.ToString() ?? "",
                        Remark = row.GetCell(10)?.ToString() ?? "",
                        Status = "正常",
                        Desinger=Utils.user.UserName,
                        DesignCompletionTime=createDt
                    };

                    materials.Add(material);

                }
            }
            return materials;
        }
        /// <summary>
        /// 检查Excel中是否有重复的数据（项目ID、项目名称、设备名称、物料编码），如果有则标红;检查必填列是否未填写，如果有标黄。
        /// </summary>
        /// <param name="filePath">Excel路径</param>
        /// <param name="materials">数据库中的Material集合</param>
        /// <returns>true：检查通过 false：未通过</returns>
        public static bool CheckExcelComponents(string filePath, List<Material> materials)
        {
            bool flag = true;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);

                // 创建红色背景样式（重复行）
                ICellStyle redStyle = workbook.CreateCellStyle();
                redStyle.FillForegroundColor = IndexedColors.Red.Index;
                redStyle.FillPattern = FillPattern.SolidForeground;

                // 创建黄色背景样式（必填项为空）
                ICellStyle yellowStyle = workbook.CreateCellStyle();
                yellowStyle.FillForegroundColor = IndexedColors.Yellow.Index;
                yellowStyle.FillPattern = FillPattern.SolidForeground;

                IRow row0 = sheet.GetRow(0);
                ICell cellB1 = row0.GetCell(1);
                ICell cellD1= row0.GetCell(3);
                ICell cellF1 = row0.GetCell(5);

                if (cellB1 == null || string.IsNullOrWhiteSpace(cellB1.ToString()))
                {
                    if (cellB1 == null)
                    {
                        cellB1=row0.CreateCell(1);
                    }
                    cellB1.CellStyle = yellowStyle;
                    flag = false;
                }
                if (cellD1 == null || string.IsNullOrWhiteSpace(cellD1.ToString()))
                {
                    if (cellD1 == null)
                    {
                        cellD1 = row0.CreateCell(3);
                    }
                    cellD1.CellStyle = yellowStyle;
                    flag = false;
                }
                if (cellF1 == null || string.IsNullOrWhiteSpace(cellF1.ToString()))
                {
                    if (cellF1 == null)
                    {
                        cellF1 = row0.CreateCell(5);
                    }
                    cellF1.CellStyle = yellowStyle;
                    flag = false;
                }

                string projectID = row0.GetCell(1).ToString();
                string projectName = row0.GetCell(3).ToString();
                string deviceName = row0.GetCell(5).ToString();



                // 遍历Excel行
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    // 检查必填列是否为空，空则标黄
                    int[] requiredIndexes = { 2 };
                    foreach (int idx in requiredIndexes)
                    {
                        ICell cell = row.GetCell(idx);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                        {
                            if (cell == null) cell = row.CreateCell(idx);
                            cell.CellStyle = yellowStyle;
                            flag = false;
                        }

                    }

                    string materialID = row.GetCell(2)?.ToString();
                    // 判断是否在传入的List中存在相同数据（项目编号、项目名称、设备名称、物料编码）
                    if (materials.Any(m => m.ProjectID == projectID && m.ProjectName == projectName && m.DeviceName == deviceName && m.MaterialID == materialID))
                    {
                        //项目编号，项目名称，设备名称，物料编号重复
                        row0.GetCell(1).CellStyle = redStyle;
                        row0.GetCell(3).CellStyle = redStyle;
                        row0.GetCell(5).CellStyle = redStyle;
                        // 给整行设置红色背景
                        foreach (var cell in row.Cells)
                        {
                            cell.CellStyle = redStyle;
                        }
                        flag = false;
                    }
                }

                // 保存Excel文件
                using (var fsWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fsWrite);
                }
            }
            return flag;
        }
    }
}
