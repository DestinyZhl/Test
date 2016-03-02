using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Model
{
    public class SystemRoleDTO:EntityDTOBase
    {
        public int Role_UID { get; set; }
        public string Role_ID { get; set; }
        public string Role_Name { get; set; }
    }
}
