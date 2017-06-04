using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticketer.Database;

namespace Ticketer.Controllers
{
    public class SettingsController : Controller
    {
        private readonly TicketContext _context;

        public SettingsController(TicketContext context)
        {
            _context = context;    
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        public async Task<IActionResult> CompanyList()
        {
            return View(await _context.Company.ToListAsync());
        }

        public async Task<IActionResult> CompanyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .SingleOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
        
        public IActionResult CompanyCreate()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyCreate([Bind("Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction("CompanyList");
            }
            return View(company);
        }
        
        public async Task<IActionResult> CompanyEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.SingleOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyEdit(int id, [Bind("Id,Name")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CompanyList");
            }
            return View(company);
        }
        
        public async Task<IActionResult> CompanyDeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.SingleOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            ViewData["Id"] = id;
            return PartialView("_DeleteCompanyConfirm");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyDelete(int id)
        {
            var company = await _context.Company.SingleOrDefaultAsync(m => m.Id == id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("CompanyList");
        }
        
        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }




        public async Task<IActionResult> GroupList()
        {
            return View(await _context.Groups
                .Include(group => group.Company)
                .ToListAsync());
        }
        
        public async Task<IActionResult> GroupDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(singleGroup => singleGroup.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }
        
        public IActionResult GroupCreate()
        {
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupCreate([Bind("Id,Name,CompanyId")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction("GroupList");
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name", @group.CompanyId);
            return View(@group);
        }
        
        public async Task<IActionResult> GroupEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name", @group.CompanyId);
            return View(@group);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupEdit(int id, [Bind("Id,Name,CompanyId")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("GroupList");
            }
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name", @group.CompanyId);
            return View(@group);
        }

        public async Task<IActionResult> GroupDeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);

            if (@group == null)
            {
                return NotFound();
            }

            ViewData["Id"] = id;
            return PartialView("_DeleteGroupConfirm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupDelete(int id)
        {
            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction("GroupList");
        }
        
        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }



        public async Task<IActionResult> UserList()
        {
            return View(await _context.User.ToListAsync());
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(string id, [Bind("FirstName,LastName,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UserList");
            }
            return View(user);
        }

        public async Task<IActionResult> UserDeleteConfirm(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["Id"] = id;
            return PartialView("_DeleteUserConfirm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserList");
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
