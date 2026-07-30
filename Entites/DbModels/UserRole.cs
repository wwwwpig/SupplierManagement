using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("UserRole")]
    public class UserRole
    {
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ID" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="UserID"    )]
         public int? UserID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="RoleID"    )]
         public int? RoleID { get; set; }
    }
}
