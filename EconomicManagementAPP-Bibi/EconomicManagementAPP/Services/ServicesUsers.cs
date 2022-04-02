using System.Security.Claims;

namespace EconomicManagementAPP.Services
{

    public interface IServicesUsers
    {
        int getUsersId();
    }
    public class ServicesUsers : IServicesUsers
    {
        private readonly HttpContext httpContext;

        public ServicesUsers(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }

        public int getUsersId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User
                        .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("User is not authenticated");
            }

        }
    }
}
