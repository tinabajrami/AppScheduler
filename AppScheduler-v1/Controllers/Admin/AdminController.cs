using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AppScheduler_v1.Areas.Identity.Data;

namespace AppScheduler_v1.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, "Patient");
                await userManager.AddToRoleAsync(user, "Doctor");
            }

            return RedirectToAction("Index", "UserManagement");
        }
    }
}
