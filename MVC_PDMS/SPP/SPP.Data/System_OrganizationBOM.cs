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
    
    public partial class System_OrganizationBOM
    {
        public int OrganizationBOM_UID { get; set; }
        public Nullable<int> ParentOrg_UID { get; set; }
        public int Order_Index { get; set; }
        public int ChildOrg_UID { get; set; }
        public System.DateTime Begin_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        public virtual System_Organization System_Organization { get; set; }
        public virtual System_Organization System_Organization1 { get; set; }
        public virtual System_Users System_Users { get; set; }
    }
}