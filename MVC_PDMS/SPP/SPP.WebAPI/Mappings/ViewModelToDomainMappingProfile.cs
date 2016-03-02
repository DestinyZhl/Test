using AutoMapper;
using SPP.Data;
using SPP.Model;

namespace SPP.WebAPI.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SystemUserDTO, System_Users>();
            Mapper.CreateMap<SystemFunctionDTO, System_Function>();
            Mapper.CreateMap<PageUnauthorizedElement, PageUnauthorizedElementEntity>();
            Mapper.CreateMap<SystemPlantDTO, System_Plant>();
            Mapper.CreateMap<SystemRoleDTO, System_Role>();
            Mapper.CreateMap<FunctionWithSubs, System_Function>();
            Mapper.CreateMap<SystemFunctionSubDTO, System_FunctionSub>();
            Mapper.CreateMap<SystemOrgDTO, System_Organization>();
            Mapper.CreateMap<SystemOrgBomDTO, System_OrganizationBOM>();
            Mapper.CreateMap<SystemUserRoleDTO, System_User_Role>();
            Mapper.CreateMap<ProductDataViewDTO, Product_Input>();
            Mapper.CreateMap<ProductDataDTO, Product_Input>();
        }
    }
}