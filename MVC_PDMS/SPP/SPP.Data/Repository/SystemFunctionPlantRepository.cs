using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System.Linq;
using System.Data.Entity.SqlServer;
using System;
using SPP.Common.Constants;
using System.Collections.Generic;

namespace SPP.Data.Repository
{
    public interface ISystemFunctionPlantRepository : IRepository<System_Function_Plant>
    {
        IQueryable<FuncPlantMaintanance> QueryFuncPlants(FuncPlantSearchModel search, Page page, out int count);
        FuncPlantMaintanance QueryFuncPlant(int uuid);
    }

    public class SystemFunctionPlantRepository : RepositoryBase<System_Function_Plant>, ISystemFunctionPlantRepository
    {
        public SystemFunctionPlantRepository(IDatabaseFactory databaseFactory)
        : base(databaseFactory)
        {

        }
        public IQueryable<FuncPlantMaintanance> QueryFuncPlants(FuncPlantSearchModel search, Page page, out int count)
        {
            var query = from plant in DataContext.System_Function_Plant.Include("System_Users")
                        select new FuncPlantMaintanance
                        {
                            System_FuncPlant_UID = plant.System_FunPlant_UID,
                            FunPlant = plant.FunPlant,
                            Plant = plant.System_Plant.Plant,
                            OPType = plant.OP_Types,
                            Plant_Manager = plant.FunPlant_Manager,
                            FuncPlant_Context = plant.FunPlant_Contact,
                            Modified_UserName = plant.System_Users.User_Name,
                            Modified_UserNTID = plant.System_Users.User_NTID,
                            Modified_Date = DateTime.Now
                        };
            if (string.IsNullOrEmpty(search.ExportUIds))
            {
                #region Modified_Date
                if (search.Modified_Date_From != null)
                {
                    query = query.Where(m => m.Modified_Date >= search.Modified_Date_From);
                }
                if (search.Modified_Date_End != null)
                {
                    var endDate = ((DateTime)search.Modified_Date_End).AddDays(1);
                    query = query.Where(m => m.Modified_Date < endDate);
                }
                #endregion

                #region 查询Modified_NTID
                if (!string.IsNullOrWhiteSpace(search.Modified_By_NTID))
                {
                    query = query.Where(q => q.Modified_UserNTID == search.Modified_By_NTID);
                }
                #endregion


                if (!string.IsNullOrWhiteSpace(search.FunPlant))
                {
                    query = query.Where(p => p.FunPlant == search.FunPlant);

                }
                if (!string.IsNullOrWhiteSpace(search.FuncPlant_Manager))
                {
                    query = query.Where(p => p.Plant_Manager == search.FuncPlant_Manager);
                }

                if (!string.IsNullOrWhiteSpace(search.OPType))
                {
                    query = query.Where(p => p.OPType == search.OPType);
                }

                if (!string.IsNullOrWhiteSpace(search.Plant))
                {
                    query = query.Where(p => p.Plant == search.Plant);
                }

                count = query.Count();
                return query.OrderBy(o => o.System_FuncPlant_UID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.System_FuncPlant_UID));

                count = 0;
                return query.OrderBy(o => o.FunPlant);
            }
        }

        public FuncPlantMaintanance QueryFuncPlant(int uuid)
        {
            var query = from plant in DataContext.System_Function_Plant.Include("System_Users")
                        where (plant.System_FunPlant_UID==uuid)
                        select new FuncPlantMaintanance
                        {
                            System_FuncPlant_UID = plant.System_FunPlant_UID,
                            FunPlant = plant.FunPlant,
                            Plant = plant.System_Plant.Plant,
                            OPType = plant.OP_Types,
                            Plant_Manager = plant.FunPlant_Manager,
                            FuncPlant_Context = plant.FunPlant_Contact,
                            Modified_UserName = plant.System_Users.User_Name,
                            Modified_UserNTID = plant.System_Users.User_NTID,
                            Modified_Date = DateTime.Now
                        };
            return query.FirstOrDefault();
        }
    }
}
