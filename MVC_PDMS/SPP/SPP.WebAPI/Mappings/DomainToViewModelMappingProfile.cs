using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPP.Data;
using SPP.Model;
using SPP.Model.ViewModels.Settings;
using SPP.Model.ViewModels;

namespace SPP.WebAPI.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<System_FunctionSub, SubFunction>();

            Mapper.CreateMap<System_Users, SystemUserDTO>();
            Mapper.CreateMap<System_Function, SystemFunctionDTO>();
            Mapper.CreateMap<System_BU_M, SystemBUMDTO>();
            Mapper.CreateMap<SystemBUMDTO,System_BU_M>();
            Mapper.CreateMap<SystemBUDDTO, System_BU_D>();
            Mapper.CreateMap<System_BU_D,SystemBUDDTO>();
            Mapper.CreateMap<BUDetailModelGet, System_BU_D>();
            Mapper.CreateMap<System_BU_D,BUDetailModelGet>();
            Mapper.CreateMap<System_BU_D, CustomBUDDTO>();
            Mapper.CreateMap<System_User_Business_Group,SystemUserBusinessGroupDTO>();

            Mapper.CreateMap<PageUnauthorizedElementEntity, PageUnauthorizedElement>();
            Mapper.CreateMap<System_Plant, SystemPlantDTO>();
            Mapper.CreateMap<System_Role, SystemRoleDTO>();
            //Mapper.CreateMap<System_FunctionSub, SystemFunctionSubDTO>();
            Mapper.CreateMap<System_Organization, SystemOrgDTO>();
            Mapper.CreateMap<System_OrganizationBOM, SystemOrgBomDTO>();
            Mapper.CreateMap<System_User_Role,SystemUserRoleDTO >();
            Mapper.CreateMap<FlowChart_Master, FlowChartMasterDTO>();
            Mapper.CreateMap<FlowChartMasterDTO,FlowChart_Master>();

            Mapper.CreateMap<FlowChartDetailDTO, FlowChart_Detail>();
            Mapper.CreateMap<FlowChart_Detail, FlowChartDetailDTO>();
            Mapper.CreateMap<List<FlowChartDetailDTO>, List<FlowChart_Detail>>();


            Mapper.CreateMap<FlowChartMgDataDTO, FlowChart_MgData>();
            Mapper.CreateMap<FlowChart_MgData, FlowChartMgDataDTO>();
            Mapper.CreateMap<FlowChart_Detail, FlowChartDetailAndMGDataDTO>();
            Mapper.CreateMap<FlowChart_Detail_Temp, FlowChartDetailAndMGDataDTO>();
            Mapper.CreateMap<FlowChart_Detail_Temp, FlowChartDetailTempDTO>();
            Mapper.CreateMap<FlowChart_MgData_Temp, FlowChartMgDataTempDTO>();
            Mapper.CreateMap<FlowChartDetailDTO, FlowChart_Detail_Temp>();
            Mapper.CreateMap<FlowChartMgDataDTO, FlowChart_MgData_Temp>();

            Mapper.CreateMap<FlowChartDetailDTO, FlowChartDetailTempDTO>();
            Mapper.CreateMap<FlowChartDetailTempDTO, FlowChartDetailDTO>();
            Mapper.CreateMap<FlowChartMgDataDTO, FlowChartMgDataTempDTO>();
            Mapper.CreateMap<FlowChartMgDataTempDTO, FlowChartMgDataDTO>();
            Mapper.CreateMap<List<SystemFunctionPlantDTO>, List<System_Function_Plant>>();

            Mapper.CreateMap<System_Project, SystemProjectDTO>();

            Mapper.CreateMap<Enumeration, EnumVM>();
            Mapper.CreateMap<System_Function_Plant, SystemFunctionPlantDTO>();
            Mapper.CreateMap<SystemFunctionPlantDTO, System_Function_Plant>();
            Mapper.CreateMap<Product_Input, ProductDataDTO>();
            Mapper.CreateMap<Product_Input, ProductDataViewDTO>();
            
            Mapper.CreateMap<PagedListModel<ProductDataDTO>, ProductDataList>();
            Mapper.CreateMap<PagedListModel<ProductDataDTO>, List<ProductDataDTO>>();
            Mapper.CreateMap<PagedListModel<ProductDataDTO>, List<ProductDataItem>>();
            Mapper.CreateMap<ZeroProcessDataSearch, ProcessDataSearch>(); 
            


        }
    }
}