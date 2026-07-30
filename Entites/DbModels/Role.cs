using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("Role")]
    public class Role
    {
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ID" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="RoleName"    )]
         public string RoleName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="CreateDt"    )]
         public DateTime? CreateDt { get; set; }
    }
}
