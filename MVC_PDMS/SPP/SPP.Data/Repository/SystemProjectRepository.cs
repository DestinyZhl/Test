using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SPP.Data.Repository
{

    public class SystemProjectRepository : RepositoryBase<System_Project>, ISystemProjectRepository
    {
        public SystemProjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            
        }
        public IQueryable<string> QueryDistinctProject(string customer)
        {
            var query = from bud in DataContext.System_BU_D
                         join project in DataContext.System_Project on bud.BU_D_UID equals project.BU_D_UID
                         where (bud.BU_D_Name == customer)
                         select (project.Project_Name);
            return query.Distinct();
        }
        public IQueryable<string> QueryDistinctProductPhase(string customername, string projectname)
        {
            var query = from bud in DataContext.System_BU_D
                        join project in DataContext.System_Project on bud.BU_D_UID equals project.BU_D_UID
                        where (bud.BU_D_Name == customername && project.Project_Name == projectname)
                        select (project.Product_Phase);
            return query.Distinct();
        }
    }
    public interface ISystemProjectRepository : IRepository<System_Project>
    {
        IQueryable<string> QueryDistinctProject(string customer);
        IQueryable<string> QueryDistinctProductPhase(string customer, string project);
    }
}
