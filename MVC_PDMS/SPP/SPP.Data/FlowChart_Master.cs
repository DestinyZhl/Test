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
    
    public partial class FlowChart_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlowChart_Master()
        {
            this.FlowChart_Detail = new HashSet<FlowChart_Detail>();
            this.FlowChart_Detail_Temp = new HashSet<FlowChart_Detail_Temp>();
            this.Product_Input = new HashSet<Product_Input>();
            this.Product_Input_History = new HashSet<Product_Input_History>();
        }
    
        public int FlowChart_Master_UID { get; set; }
        public int Project_UID { get; set; }
        public string Part_Types { get; set; }
        public int FlowChart_Version { get; set; }
        public string FlowChart_Version_Comment { get; set; }
        public bool Is_Latest { get; set; }
        public bool Is_Closed { get; set; }
        public int Modified_UID { get; set; }
        public System.DateTime Modified_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlowChart_Detail> FlowChart_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlowChart_Detail_Temp> FlowChart_Detail_Temp { get; set; }
        public virtual System_Project System_Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Input> Product_Input { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Input_History> Product_Input_History { get; set; }
        public virtual System_Users System_Users { get; set; }
    }
}
