using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("User")]
    public class User
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
         public string UserID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="UserName"    )]
         public string UserName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Pwd"    )]
         public string Pwd { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Remark"    )]
         public string Remark { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="CreateDT"    )]
         public DateTime? CreateDT { get; set; }
    }
}
