using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("Supplier")]
    public class Supplier
    {
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="SupplierName" ,IsPrimaryKey = true   )]
         public string SupplierName { get; set; }
    }
}
