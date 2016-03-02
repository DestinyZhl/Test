using SPP.Data.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Data;
using SPP.Model;
using SPP.Model.ViewModels;

namespace SPP.Data.Repository
{
    public class WIPChangeHistoryRepository : RepositoryBase<WIP_Change_History>, IWIPChangeHistoryRepository
    {
        public WIPChangeHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }

    public interface IWIPChangeHistoryRepository : IRepository<WIP_Change_History>
    {

    }
}
