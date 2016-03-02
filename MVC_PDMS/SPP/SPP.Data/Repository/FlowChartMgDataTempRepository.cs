using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP.Data.Infrastructure;

namespace SPP.Data.Repository
{
    public interface IFlowChartMgDataTempRepository : IRepository<FlowChart_MgData_Temp>
    {

    }

    public class FlowChartMgDataTempRepository : RepositoryBase<FlowChart_MgData_Temp>, IFlowChartMgDataTempRepository
    {
        public FlowChartMgDataTempRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
