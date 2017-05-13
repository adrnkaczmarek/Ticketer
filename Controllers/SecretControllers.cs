using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Database;
using Ticketer.Models;

namespace Ticketer.Controllers
{
    [Authorize]
    public class SecretController : Controller
    {
        private UserManager<User> UserManager { get; }

        public SecretController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(userName);

            var model = new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(model);
        }
    }
}