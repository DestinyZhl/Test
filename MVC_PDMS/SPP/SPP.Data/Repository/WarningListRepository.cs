using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;

namespace SPP.Data.Repository
{

    public class WarningListRepository : RepositoryBase<Warning_List>, IWarningListRepository
    {
        public WarningListRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        /// <summary>
        /// Get warning list
        /// </summary>
        /// <param name="user_account_uid">user_account_uid</param>
        /// <param name="count">number of total records</param>
        /// <returns></returns>
        public IEnumerable<WarningListDTO> GetWarninglistDatas(int user_account_uid, out int count)
        {
            var query = (from warninglist in DataContext.Warning_List
                        join FunPlant in DataContext.System_Function_Plant on warninglist.FncPlant_Now equals FunPlant.System_FunPlant_UID
                        join UserFunPlant in DataContext.System_User_FunPlant on FunPlant.System_FunPlant_UID equals UserFunPlant.System_FunPlant_UID
                        join UserRole in DataContext.System_User_Role on UserFunPlant.Account_UID equals UserRole.Account_UID
                        join Role in DataContext.System_Role on UserRole.Role_UID equals Role.Role_UID
                        where (UserFunPlant.Account_UID == user_account_uid || Role.Role_Name == "系统管理員")
                        select new WarningListDTO
                        {
                            Warning_UID = warninglist.Warning_UID,
                            Part_Types = warninglist.Part_Types,
                            Product_Date = warninglist.Product_Date,
                            Product_Phase = warninglist.Product_Phase,
                            Project = warninglist.Project,
                            Customer = warninglist.Customer,
                            FncPlant_Effect = FunPlant.FunPlant,
                            Time_Interval=warninglist.Time_Interval
                        }).Distinct();
            count = query.Count();


            return query.OrderBy(o => o.Product_Date);

        }

        public IEnumerable<ProcessDataSearch> GetWarningDataByWarningUid(int WarningUId)
        {
            var query = from warninglist in DataContext.Warning_List
                        join FunPlant in DataContext.System_Function_Plant on warninglist.FncPlant_Now equals FunPlant.System_FunPlant_UID
                        where warninglist.Warning_UID==WarningUId
                        select new ProcessDataSearch
                        {
                            Part_Types = warninglist.Part_Types,
                            Product_Phase = warninglist.Product_Phase,
                            Project = warninglist.Project,
                            Customer = warninglist.Customer,
                            QuertFlag="WarningList",
                            Date=warninglist.Product_Date,
                            Func_Plant=FunPlant.FunPlant,
                            Time=warninglist.Time_Interval
                        };
            return query;
        }
    }

    public interface IWarningListRepository : IRepository<Warning_List>
    {
        /// <summary>
        /// Get warning list
        /// </summary>
        /// <param name="user_account_uid">user_account_uid</param>
        /// <param name="count">number of total records</param>
        /// <returns>paged records</returns>
        IEnumerable<WarningListDTO> GetWarninglistDatas(int user_account_uid, out int count);

        IEnumerable<ProcessDataSearch> GetWarningDataByWarningUid(int WarningUId);
    }
}
