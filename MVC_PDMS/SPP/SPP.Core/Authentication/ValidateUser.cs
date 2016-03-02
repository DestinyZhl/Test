using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Core.Authentication
{
    public class ValidateUser
    {
        public static bool LDAPValidate(string userName, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["LDAPPath"].ToString(), userName, password);
                
            try
            {
                string objectSid = (new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0).Value);
                return objectSid != null;
            }
            catch // directory services COMException
            {
                return false;
            }
            finally // release unmanaged resource
            {
                entry.Dispose();
            }
        }
    }
}
