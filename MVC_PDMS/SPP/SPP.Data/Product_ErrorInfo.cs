//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPP.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product_ErrorInfo
    {
        public int ErrorInfo_UID { get; set; }
        public int Product_UID { get; set; }
        public Nullable<int> Good_MismatchFlag { get; set; }
        public Nullable<int> Picking_MismatchFlag { get; set; }
        public string FunPlant_Now { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        public virtual Product_Input_History Product_Input_History { get; set; }
        public virtual Product_Input Product_Input { get; set; }
        public virtual System_Function_Plant System_Function_Plant { get; set; }
        public virtual System_Function_Plant System_Function_Plant1 { get; set; }
        public virtual System_Users System_Users { get; set; }
    }
}
