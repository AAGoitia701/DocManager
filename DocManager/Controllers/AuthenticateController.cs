using DocManager.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocManager.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticateController(UserManager<ApplicationUser> userMngr)
        {
            _userManager = userMngr;
        }

        [Authorize]
        public async Task<IActionResult> Secured()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return View((object)$"Hello, {user.Nombre} you are IN!");
        }
    }
}
