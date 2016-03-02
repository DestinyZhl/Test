using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System.Linq;
using System;
using System.Data.Entity.SqlServer;
using SPP.Common.Enums;

namespace SPP.Data.Repository
{
    public class SystemOrgRepository : RepositoryBase<System_Organization>, ISystemOrgRepository
    {
        public SystemOrgRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
        /// <summary>
        /// Query system users and order by account_id asc
        /// </summary>
        /// <param name="search">search model</param>
        /// <param name="page">page info</param>
        /// <param name="count">number of total records</param>
        /// <returns></returns>
        public IQueryable<SystemOrgDTO> QueryOrgs(OrgModelSearch search, Page page, out int count)
        {
            var query = from Org in DataContext.System_Organization.Include("System_Users")
                        select new SystemOrgDTO
                        {
                            Organization_UID = Org.Organization_UID,
                            Organization_ID = Org.Organization_ID,
                            Organization_Name = Org.Organization_Name,
                            Organization_Desc = Org.Organization_Desc,
                            Begin_Date = Org.Begin_Date,
                            End_Date = Org.End_Date,
                            OrgManager_Name = Org.OrgManager_Name,
                            OrgManager_Tel = Org.OrgManager_Tel,
                            OrgManager_Email = Org.OrgManager_Email,
                            Cost_Center = Org.Cost_Center,
                            Modified_Date = Org.Modified_Date,
                            Modified_UserName = Org.System_Users.User_Name,
                            Modified_UserNTID = Org.System_Users.User_NTID
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

                if (!string.IsNullOrWhiteSpace(search.Organization_ID))
                {
                    query = query.Where(p => p.Organization_ID == search.Organization_ID);
                }
                if (!string.IsNullOrWhiteSpace(search.Organization_Name))
                {
                    query = query.Where(p => p.Organization_Name.Contains(search.Organization_Name));
                }
                if (!string.IsNullOrWhiteSpace(search.Organization_Desc))
                {
                    query = query.Where(p => p.Organization_Desc.Contains(search.Organization_Desc));
                }
                if (!string.IsNullOrWhiteSpace(search.OrgManager_Name))
                {
                    query = query.Where(p => p.OrgManager_Name.Contains(search.OrgManager_Name));
                }
                if (!string.IsNullOrWhiteSpace(search.Cost_Center))
                {
                    query = query.Where(p => p.Cost_Center == search.Cost_Center);
                }
                if (!string.IsNullOrWhiteSpace(search.Modified_By_NTID))
                {
                    query = query.Where(q => q.Modified_UserNTID == search.Modified_By_NTID);
                }
                count = query.Count();
                return query.OrderBy(o => o.Organization_ID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.Organization_UID));

                count = 0;
                return query.OrderBy(o => o.Organization_ID);
            }
        }

        public IQueryable<System_Organization> GetValidOrgsByUserUId(int accountUId)
        {
            var now = DateTime.Now.Date;

            var query = from org in DataContext.System_Organization
                        join user_org in DataContext.System_UserOrg on org.Organization_UID equals user_org.Organization_UID
                        where user_org.Account_UID == accountUId
                            && ((user_org.Begin_Date <= now && !user_org.End_Date.HasValue)
                                || (user_org.Begin_Date <= now && user_org.End_Date >= now))
                        select org;

            return query;
        }

    }

    public interface ISystemOrgRepository : IRepository<System_Organization>
    {
        /// <summary>
        /// Query system Org and order by Org asc
        /// </summary>
        /// <param name="search">search model</param>
        /// <param name="page">page info</param>
        /// <param name="count">number of total records</param>
        /// <returns>paged records</returns>
        IQueryable<SystemOrgDTO> QueryOrgs(OrgModelSearch search, Page page, out int count);

        IQueryable<System_Organization> GetValidOrgsByUserUId(int accountUId);

    }
}
