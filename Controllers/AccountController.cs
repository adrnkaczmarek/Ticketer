using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticketer.Database;
using Ticketer.Models;
using PutNet.Web.Identity.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticketer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TicketContext _context;
        private UserManager<User> UserManager { get; }
        private SignInManager<User> SignInManager { get; }

        public AccountController(TicketContext context, UserManager<User> userManager, SignInManager<User> signInManager) 
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GroupId = model.GroupId
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }
            
            return RedirectToAction(nameof(SettingsController.UserList), "Settings");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if(result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else if(result.RequiresTwoFactor)
            {
                throw new NotImplementedException();
            }
            else if(result.IsLockedOut)
            {
                throw new NotImplementedException();
            }
            else
            {
                ModelState.AddModelError("", "Invalid user name or password.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.LogIn), "Account");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(TicketsController.Index), "Tickets");
            }
        }
    }
}
