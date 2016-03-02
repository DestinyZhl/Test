using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model;
using System.Collections.Generic;
using System.Linq;


namespace SPP.Service
{
    public interface IChartService
    {
        List<string> GetFunPlant(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color);

        List<string> GetProcess(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color, string FunPlant);

    }

    public class ChartService : IChartService
    {
        #region Private interfaces properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductInputRepository productInputRepository;
        private readonly IFlowChartDetailRepository FlowChartDetailRepository;
        #endregion //Private interfaces properties

        #region Service constructor
        public ChartService(
        IProductInputRepository productInputRepository, IFlowChartDetailRepository FlowChartDetailRepository,
            IUnitOfWork unitOfWork)
        {
            this.productInputRepository = productInputRepository;
            this.FlowChartDetailRepository = FlowChartDetailRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion //Service constructor

        public List<string> GetFunPlant(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color)
        {
            List<string> result = new List<string>();
            result.Add("ALL");
            var EnumEntity = FlowChartDetailRepository.QueryFunPlant(CustomerName, ProjectName, ProductPhaseName, PartTypesName, Color);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            result.AddRange(customerList);
            return result;
        }

        public List<string> GetProcess(string CustomerName, string ProjectName, string ProductPhaseName,
            string PartTypesName, string Color, string FunPlant)
        {
            List<string> result = new List<string>();
            result.Add("ALL");
            var EnumEntity = FlowChartDetailRepository.QueryProcess(CustomerName, ProjectName, ProductPhaseName, PartTypesName, Color,FunPlant);
            var customerList = AutoMapper.Mapper.Map<List<string>>(EnumEntity);
            result.AddRange(customerList);
            return result;
        }
    }
}
