using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Linq;

namespace SPP.Data.Repository
{
    public class SystemUserRepository : RepositoryBase<System_Users>, ISystemUserRepository
    {
        public SystemUserRepository(IDatabaseFactory databaseFactory)
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
        public IQueryable<SystemUserDTO> QueryUsers(UserModelSearch search, Page page, out int count)
        {
            var query = from user in DataContext.System_Users
                        join modifiedUser in DataContext.System_Users on user.Modified_UID equals modifiedUser.Account_UID
                        select new SystemUserDTO {
                            Account_UID = user.Account_UID,
                            User_NTID =user.User_NTID,
                            User_Name = user.User_Name,
                            Email =user.Email,
                            Enable_Flag =user.Enable_Flag,
                            Tel=user.Tel,
                            Modified_UserName = modifiedUser.User_Name,
                            Modified_UserNTID = modifiedUser.User_NTID,
                            Modified_Date=user.Modified_Date,
                            Modified_UID = user.Modified_UID
                        };

            if (string.IsNullOrEmpty(search.ExportUIds))
            {
                if (search.Account_UID > 0)
                {
                    query = query.Where(p => p.Account_UID == search.Account_UID);
                }
                if (!string.IsNullOrWhiteSpace(search.User_NTID))
                {
                    query = query.Where(p => p.User_NTID == search.User_NTID);
                }
                if (!string.IsNullOrWhiteSpace(search.User_Name))
                {
                    query = query.Where(p => p.User_Name.Contains(search.User_Name));
                }
                if (!string.IsNullOrWhiteSpace(search.Tel))
                {
                    query = query.Where(p => p.Tel.Contains(search.Tel));
                }
                if (!string.IsNullOrWhiteSpace(search.Email))
                {
                    query = query.Where(p => p.Email.Contains(search.Email));
                }
                if (search.Enable_Flag != null)
                {
                    query = query.Where(p => p.Enable_Flag == search.Enable_Flag);
                }
                if (!string.IsNullOrWhiteSpace(search.Modified_By))
                {
                    query = query.Where(p => p.Modified_UserNTID == search.Modified_By);
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

                return query.OrderBy(o => o.Account_UID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.Account_UID));

                count = 0;
                return query.OrderBy(o => o.Account_UID);
            }
        }
    }

    public interface ISystemUserRepository : IRepository<System_Users>
    {
        /// <summary>
        /// Query system users and order by account_id asc
        /// </summary>
        /// <param name="search">search model</param>
        /// <param name="page">page info</param>
        /// <param name="count">number of total records</param>
        /// <returns>paged records</returns>
        IQueryable<SystemUserDTO> QueryUsers(UserModelSearch search, Page page, out int count);
    }
}
