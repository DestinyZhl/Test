using System;
using System.Linq;
using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;

namespace SPP.Data.Repository
{
    public class SystemRoleRepository : RepositoryBase<System_Role>,  ISystemRoleRepository
    {
        public SystemRoleRepository(IDatabaseFactory databaseFactory)
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
        public IQueryable<System_Role> QueryRoles(RoleModelSearch search, Page page, out int count)
        {
            var query = from role in DataContext.System_Role.Include("System_Users")
                        select role;

            if (string.IsNullOrEmpty(search.ExportUIds))
            {
                if (!string.IsNullOrWhiteSpace(search.Role_ID))
                {
                    query = query.Where(p => p.Role_ID == search.Role_ID);
                }
                if (!string.IsNullOrWhiteSpace(search.Role_Name))
                {
                    query = query.Where(p => p.Role_Name.Contains(search.Role_Name));
                }
                if (!string.IsNullOrWhiteSpace(search.Modified_By))
                {
                    query = query.Where(p => p.System_Users.User_Name == search.Modified_By);
                }
                if (search.Modified_Date_From != null)
                {
                    query = query.Where(p => p.Modified_Date >= search.Modified_Date_From);
                }
                if (search.Modified_Date_End != null)
                {
                    var endDate = ((DateTime)search.Modified_Date_End).AddDays(1);
                    query = query.Where(p => p.Modified_Date < endDate);
                }
                count = query.Count();

                return query.OrderBy(o => o.Role_UID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.Role_UID));

                count = 0;
                return query
                            .OrderBy(o => o.Role_UID)
                            .ThenBy(o => o.Role_ID);
            }
                
        }


        /// <summary>
        /// add by tonny 2015-11-24
        /// get role functions by role uid
        /// </summary>
        /// <param name="role_uid"></param>
        /// <returns></returns>
        public System_Role QueryRoleFunctionsByRoleUID(int role_uid)
        {
            var query = from role in DataContext.System_Role
                        join role_func in DataContext.System_Role_Function on role.Role_UID equals role_func.Role_UID
                        join func in DataContext.System_Function on role_func.Function_UID equals func.Function_UID
                        where role.Role_UID == role_uid
                        select role;

            return query.FirstOrDefault();
        }
    }

    public interface ISystemRoleRepository : IRepository<System_Role>
    {
        IQueryable<System_Role> QueryRoles(RoleModelSearch search, Page page, out int count);
        System_Role QueryRoleFunctionsByRoleUID(int role_uid);
    }
}
