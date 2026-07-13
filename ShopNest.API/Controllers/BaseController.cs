using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShopNest.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userClaim);
        }
    }
}
