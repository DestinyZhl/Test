using SPP.Data.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Data;
using SPP.Model;
using SPP.Common.Helpers;
using SPP.Common.Enums;
using System.Data.Entity.SqlServer;

namespace SPP.Data.Repository
{
    public class SystemUserOrgRepository : RepositoryBase<System_UserOrg>, ISystemUserOrgRepository
    {
        public SystemUserOrgRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IQueryable<UserOrgItem> QueryUserOrgs(UserOrgModelSearch search, Page page, out int count)
        {
            var query = from userOrg in DataContext.System_UserOrg.Include("System_Users")
                        join users in DataContext.System_Users on userOrg.Account_UID equals users.Account_UID
                        join modifyusers in DataContext.System_Users on userOrg.Modified_UID equals modifyusers.Account_UID
                        join Org in DataContext.System_Organization on userOrg.Organization_UID equals Org.Organization_UID
                        select new UserOrgItem
                        {
                            System_UserOrgUID  = userOrg.System_UserOrgUID,
                            User_NTID = users.User_NTID,
                            User_Name = users.User_Name,
                            Organization_ID = Org.Organization_ID,
                            Organization_Name=Org.Organization_Name,
                            Begin_Date = userOrg.Begin_Date,
                            End_Date = userOrg.End_Date,
                            Account_UID = userOrg.Account_UID,
                            Modified_UID = userOrg.Modified_UID,
                            Modified_Date = userOrg.Modified_Date,
                            Modified_UserName = modifyusers.User_Name,    
                            Modified_UserNTID = modifyusers.User_NTID                        
                        };
            if (string.IsNullOrEmpty(search.ExportUIds))
            {
                #region Query_Types

                if (search.query_types != null && search.Reference_Date != null)
                {
                    EnumValidity queryType = (EnumValidity)Enum.ToObject(typeof(EnumValidity), search.query_types);

                    switch (queryType)
                    {
                        case EnumValidity.Valid:
                            query = query.Where(p => p.Begin_Date <= search.Reference_Date && (p.End_Date >= search.Reference_Date || p.End_Date == null));
                            break;
                        case EnumValidity.Invalid:
                            query = query.Where(p => p.Begin_Date > search.Reference_Date || (p.End_Date < search.Reference_Date && p.End_Date != null));
                            break;
                        default:
                            break;
                    }

                }
                #endregion

                #region Modified_Date

                if (search.Modified_Date_From != null)
                {
                    query = query.Where(m => SqlFunctions.DateDiff("dd", m.Modified_Date, search.Modified_Date_From) <= 0);
                }
                if (search.Modified_Date_End != null)
                {
                    query = query.Where(m => SqlFunctions.DateDiff("dd", m.Modified_Date, search.Modified_Date_End) >= 0);
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(search.User_NTID))
                {
                    query = query.Where(p => p.User_NTID == search.User_NTID);

                }
                if (!string.IsNullOrWhiteSpace(search.User_Name))
                {
                    query = query.Where(p => p.User_Name.Contains(search.User_Name));

                }
                if (!string.IsNullOrWhiteSpace(search.Organization_ID))
                {
                    query = query.Where(p => p.Organization_ID == search.Organization_ID);

                }
                if (!string.IsNullOrWhiteSpace(search.Organization_Name))
                {
                    query = query.Where(p => p.Organization_Name.Contains(search.Organization_Name));
                }

                if (!string.IsNullOrWhiteSpace(search.Modified_By_NTID))
                {
                    query = query.Where(q => q.Modified_UserNTID == search.Modified_By_NTID);
                }

                count = query.Count();
                return query.OrderBy(o => o.User_NTID ).ThenBy(o=>o.Organization_ID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.System_UserOrgUID));

                count = 0;
                return query.OrderByDescending(o => o.Modified_Date);
            }
        }
        public IQueryable<UserOrgWithOrg> QueryUserOrgsByAccountUID(int uuid)
        {
            var query = from userOrg in DataContext.System_UserOrg
                        join Org in DataContext.System_Organization on userOrg.Organization_UID equals Org.Organization_UID 
                        join user in DataContext.System_Users on userOrg.Account_UID equals user.Account_UID                       
                        where user.Account_UID == uuid
                        select new UserOrgWithOrg
                        {                            
                            Organization_UID = Org.Organization_UID,
                            Organization_ID = Org.Organization_ID,
                            Organization_Name=Org.Organization_Name,
                            Org_Begin_Date=Org.Begin_Date,
                            Org_End_Date=Org.End_Date,
                            System_UserOrgUID = userOrg.System_UserOrgUID,
                            UserOrg_Begin_Date = userOrg.Begin_Date,
                            UserOrg_End_Date = (DateTime)userOrg.End_Date
                        };
            return query;
        }
    }
    public interface ISystemUserOrgRepository : IRepository<System_UserOrg>
    {

        IQueryable<UserOrgItem> QueryUserOrgs(UserOrgModelSearch search, Page page, out int count);
        IQueryable<UserOrgWithOrg> QueryUserOrgsByAccountUID(int uuid);
    }
}
