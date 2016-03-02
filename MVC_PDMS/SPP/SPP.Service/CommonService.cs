using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Service
{
    public interface ICommonService
    {
        #region User Part
        SystemUserDTO GetSystemUserByUId(int uid);
        SystemUserDTO GetSystemUserByNTId(string ntid);
        #endregion //User Part

        IEnumerable<SystemRoleDTO> GetAllRoles();
        IEnumerable<SystemPlantDTO> GetValidPlantsByUserUId(int accountUId);
        IEnumerable<SystemBUMDTO> GetValidBUMsByUserUId(int accountUId);
        IEnumerable<SystemBUDDTO> GetValidBUDsByUserUId(int accountUId);
        IEnumerable<SystemOrgDTO> GetValidOrgsByUserUId(int accountUId);
    }

    public class CommonService : ICommonService
    {
        #region Private interfaces properties
        private readonly IUnitOfWork unitOfWork;
        private readonly ISystemUserRepository systemUserRepository;
        private readonly ISystemRoleRepository systemRoleRepository;
        private readonly ISystemPlantRepository systemPlantRepository;
        private readonly ISystemOrgRepository systemOrgRepository;
        private readonly ISystemBUMRepository systemBUMRepository;
        private readonly ISystemBUDRepository systemBUDRepository;
        #endregion //Private interfaces properties

        #region Service constructor
        public CommonService(
            ISystemUserRepository systemUserRepository,
            ISystemRoleRepository systemRoleRepository,
            ISystemPlantRepository systemPlantRepository,
            ISystemOrgRepository systemOrgRepository,
            ISystemBUMRepository systemBUMRepository,
            ISystemBUDRepository systemBUDRepository,
            IUnitOfWork unitOfWork)
        {
            this.systemUserRepository = systemUserRepository;
            this.systemRoleRepository = systemRoleRepository;
            this.systemPlantRepository = systemPlantRepository;
            this.systemOrgRepository = systemOrgRepository;
            this.systemBUMRepository = systemBUMRepository;
            this.systemBUDRepository = systemBUDRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion //Service constructor

        #region User Part
        public SystemUserDTO GetSystemUserByUId(int uid)
        {
            var query = systemUserRepository.GetById(uid);
            SystemUserDTO returnUser = (query == null ? null : AutoMapper.Mapper.Map<SystemUserDTO>(query));
            return returnUser;
        }

        public SystemUserDTO GetSystemUserByNTId(string ntid)
        {
            var query = systemUserRepository.GetFirstOrDefault(q => q.User_NTID == ntid);
            SystemUserDTO returnUser = (query == null ? null : AutoMapper.Mapper.Map<SystemUserDTO>(query));
            return returnUser;
        }
        #endregion //User Part

        public IEnumerable<SystemRoleDTO> GetAllRoles()
        {
            var roles = systemRoleRepository.GetAll().AsEnumerable();
            return AutoMapper.Mapper.Map<IEnumerable<SystemRoleDTO>>(roles);
        }

        public IEnumerable<SystemPlantDTO> GetValidPlantsByUserUId(int accountUId)
        {
            var plants = systemPlantRepository.GetValidPlantsByUserUId(accountUId).AsEnumerable();
            return AutoMapper.Mapper.Map<IEnumerable<SystemPlantDTO>>(plants);
        }

        public IEnumerable<SystemBUMDTO> GetValidBUMsByUserUId(int accountUId)
        {
            var bums = systemBUMRepository.GetValidBUMsByUserUId(accountUId).AsEnumerable();
            return AutoMapper.Mapper.Map<IEnumerable<SystemBUMDTO>>(bums);
        }

        public IEnumerable<SystemBUDDTO> GetValidBUDsByUserUId(int accountUId)
        {
            var buds = systemBUDRepository.GetValidBUDsByUserUId(accountUId).AsEnumerable();
            return AutoMapper.Mapper.Map<IEnumerable<SystemBUDDTO>>(buds);
        }

        public IEnumerable<SystemOrgDTO> GetValidOrgsByUserUId(int accountUId)
        {
            var buds = systemOrgRepository.GetValidOrgsByUserUId(accountUId).AsEnumerable();
            return AutoMapper.Mapper.Map<IEnumerable<SystemOrgDTO>>(buds);
        }
    }
}
