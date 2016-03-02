using SPP.Core;
using SPP.Core.Authentication;
using SPP.Service;
using System.Web.Http;

namespace SPP.WebAPI.Controllers
{
    public class CommonController : ApiControllerBase
    {
        #region Private interfaces properties
        private ICommonService commonService;
        #endregion //Private interfaces properties

        #region Controller constructor
        public CommonController(ICommonService commonService)
        {
            this.commonService = commonService;
        }
        #endregion //Controller constructor

        #region System User
        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetSystemUserByUId(int uid)
        {
            var targetUser = commonService.GetSystemUserByUId(uid);

            if (targetUser == null)
            {
                return NotFound();
            }

            return Ok(targetUser);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetSystemUserByNTId(string ntid)
        {
            var targetUser = commonService.GetSystemUserByNTId(ntid);

            if (targetUser == null)
            {
                return NotFound();
            }

            return Ok(targetUser);
        }
        #endregion //System User

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetAllRoles()
        {
            var roles = commonService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetValidPlantsByUserUId(int uid)
        {
            var plants = commonService.GetValidPlantsByUserUId(uid);
            return Ok(plants);
        }
        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetValidBUMsByUserUId(int uid)
        {
            var bums = commonService.GetValidBUMsByUserUId(uid);
            return Ok(bums);
        }
        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetValidBUDsByUserUId(int uid)
        {
            var buds = commonService.GetValidBUDsByUserUId(uid);
            return Ok(buds);
        }
        [HttpGet]
        [IgnoreDBAuthorize]
        public IHttpActionResult GetValidOrgsByUserUId(int uid)
        {
            var orgs = commonService.GetValidOrgsByUserUId(uid);
            return Ok(orgs);
        }
    }
}
