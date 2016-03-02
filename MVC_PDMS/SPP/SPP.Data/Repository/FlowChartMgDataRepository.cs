using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP.Data.Infrastructure;

namespace SPP.Data.Repository
{
    public interface IFlowChartMgDataRepository : IRepository<FlowChart_MgData>
    {

    }

    public class FlowChartMgDataRepository : RepositoryBase<FlowChart_MgData>, IFlowChartMgDataRepository
    {
        public FlowChartMgDataRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
