using SPP.Core.Authentication;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPP.Core
{
    [SPPAPIAuthentication]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class ApiControllerBase : ApiController
    {

    }
}
