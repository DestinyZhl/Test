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
    
    public partial class System_Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public System_Role()
        {
            this.System_Role_Function = new HashSet<System_Role_Function>();
            this.System_User_Role = new HashSet<System_User_Role>();
        }
    
        public int Role_UID { get; set; }
        public string Role_ID { get; set; }
        public string Role_Name { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        public virtual System_Users System_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_Role_Function> System_Role_Function { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_User_Role> System_User_Role { get; set; }
    }
}