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
    
    public partial class System_Organization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public System_Organization()
        {
            this.System_OrganizationBOM = new HashSet<System_OrganizationBOM>();
            this.System_OrganizationBOM1 = new HashSet<System_OrganizationBOM>();
            this.System_UserOrg = new HashSet<System_UserOrg>();
        }
    
        public int Organization_UID { get; set; }
        public string Organization_ID { get; set; }
        public string Organization_Name { get; set; }
        public string Organization_Desc { get; set; }
        public string OrgManager_Name { get; set; }
        public string OrgManager_Tel { get; set; }
        public string OrgManager_Email { get; set; }
        public string Cost_Center { get; set; }
        public System.DateTime Begin_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        public virtual System_Users System_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_OrganizationBOM> System_OrganizationBOM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_OrganizationBOM> System_OrganizationBOM1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<System_UserOrg> System_UserOrg { get; set; }
    }
}
