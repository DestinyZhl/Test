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
    
    public partial class Warning_List
    {
        public int Warning_UID { get; set; }
        public string Warning_Types { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string Part_Types { get; set; }
        public string Product_Phase { get; set; }
        public System.DateTime Product_Date { get; set; }
        public string Time_Interval { get; set; }
        public int FncPlant_Now { get; set; }
        public int FncPlant_Effect { get; set; }
    
        public virtual System_Function_Plant System_Function_Plant { get; set; }
        public virtual System_Function_Plant System_Function_Plant1 { get; set; }
    }
}