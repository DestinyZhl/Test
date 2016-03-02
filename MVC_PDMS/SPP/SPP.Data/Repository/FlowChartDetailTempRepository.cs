using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP.Data.Infrastructure;

namespace SPP.Data.Repository
{
    public interface IFlowChartDetailTempRepository : IRepository<FlowChart_Detail_Temp>
    {

    }

    public class FlowChartDetailTempRepository : RepositoryBase<FlowChart_Detail_Temp>, IFlowChartDetailTempRepository
    {
        public FlowChartDetailTempRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
