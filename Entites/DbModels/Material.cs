using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("Material")]
    public class Material
    {
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ID" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="MaterialID"    )]
         public string MaterialID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ProjectID"    )]
         public string ProjectID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ProjectName"    )]
         public string ProjectName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="DeviceName"    )]
         public string DeviceName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="BOMLevel"    )]
         public string BOMLevel { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="MaterialName"    )]
         public string MaterialName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Qty"    )]
         public decimal? Qty { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Unit"    )]
         public string Unit { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Spec"    )]
         public string Spec { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="MaterialRole"    )]
         public string MaterialRole { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="DesignStatus"    )]
         public string DesignStatus { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="WorkmanShip"    )]
         public string WorkmanShip { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Remark"    )]
         public string Remark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Status"    )]
         public string Status { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Desinger"    )]
         public string Desinger { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="DesignCompletionTime"    )]
         public DateTime? DesignCompletionTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Buyer"    )]
         public string Buyer { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="BuyTime"    )]
         public DateTime? BuyTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PlannedDeliveryDate"    )]
         public DateTime? PlannedDeliveryDate { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="SupplierName"    )]
         public string SupplierName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="BuyerRemark"    )]
         public string BuyerRemark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="QualityInspector"    )]
         public string QualityInspector { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="QualityInspectionTime"    )]
         public DateTime? QualityInspectionTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="IsPass"    )]
         public string IsPass { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ReasonForNotPass"    )]
         public string ReasonForNotPass { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ProcessMode"    )]
         public string ProcessMode { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ReturnSupplierTime"    )]
         public DateTime? ReturnSupplierTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="QualityInspectorRemark"    )]
         public string QualityInspectorRemark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PersonIn"    )]
         public string PersonIn { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="InWarehouseTime"    )]
         public DateTime? InWarehouseTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PersonInRemark"    )]
         public string PersonInRemark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PersonOut"    )]
         public string PersonOut { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="OutWarehouseTime"    )]
         public DateTime? OutWarehouseTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PersonOutRemark"    )]
         public string PersonOutRemark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ChangeHistory"    )]
         public string ChangeHistory { get; set; }
    }
}
