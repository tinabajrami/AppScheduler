using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AppScheduler_v1.Areas.Identity.Data;

namespace AppScheduler_v1.Controllers.Admin
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public UserManagementController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
    }
}
