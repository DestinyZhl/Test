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

    public interface ISystemUserFunPlantRepository : IRepository<System_User_FunPlant>
    {
        int checkFuncPlantIsExit(int uuid);
    }

    public class SystemUserFunPlantRepository : RepositoryBase<System_User_FunPlant>, ISystemUserFunPlantRepository
    {
        public SystemUserFunPlantRepository(IDatabaseFactory databaseFactory)
        : base(databaseFactory)
        {

        }

        public int checkFuncPlantIsExit(int uuid)
        {
            var result = DataContext.System_User_FunPlant.Count(p => p.System_FunPlant_UID == uuid);
            return result;
        }
    }
}
