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
    
    public partial class System_Role_Function
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public System_Role_Function()
        {
            this.System_Role_FunctionSub = new HashSet<System_Role_FunctionSub>();
        }
    
        public int System_Role_Function_UID { get; set; }
        public int Role_UID { get; set; }
        public int Function_UID { get; set; }
        public bool Is_Show { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        public virtual System_Function System_Function { get; set; }
        public virtual System_Role System_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_Role_FunctionSub> System_Role_FunctionSub { get; set; }
        public virtual System_Users System_Users { get; set; }
    }
}
