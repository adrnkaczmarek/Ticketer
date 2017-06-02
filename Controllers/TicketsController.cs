using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticketer.Database;
using Ticketer.Database.Enums;
using Ticketer.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace PutNet.Web.Identity.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketContext _context;
        private readonly UserManager<User> _userManager;
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public TicketsController(UserManager<User> userManager, TicketContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index(TicketFilters? filter)
        {
            var tickets = await _context.Tickets
                .Include(ticket => ticket.Assigned)
                .Include(ticket => ticket.Company)
                .Include(ticket => ticket.AssignedGroup)
                .ToListAsync();
            
            if (filter != null)
            {
                switch (filter)
                {
                    case TicketFilters.Closed:
                        tickets = tickets.Where(ticket => ticket.State == TicketState.Closed).ToList();
                        break;
                    case TicketFilters.Open:
                        tickets = tickets.Where(ticket => ticket.State == TicketState.Open).ToList();
                        break;
                    case TicketFilters.Unassigned:
                        tickets = tickets.Where(ticket => ticket.Assigned == null).ToList();
                        break;
                    case TicketFilters.MyGroup:
                        User currentUser = await GetCurrentUserAsync();

                        if (currentUser.Group != null)
                        {
                            tickets = tickets.Where(ticket => ticket.AssignedGroupId == currentUser.Group.Id).ToList();
                        }
                        else
                        {
                            tickets.RemoveAll(ticket => true);
                        }

                        break;
                }
            }
           

            return View(tickets);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(currentTicket => currentTicket.Assigned)
                .Include(currentTicket => currentTicket.Company)
                .Include(currentTicket => currentTicket.AssignedGroup)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .SingleOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            ViewData["AssignedId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["AssignedGroupId"] = new SelectList(_context.Groups, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedAt,Priority,State,CompanyId,AssignedId,AssignedGroupId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["AssignedId"] = new SelectList(_context.Users, "Id", "Id", ticket.Assigned.Id);
            ViewData["AssignedGroupId"] = new SelectList(_context.Groups, "Id", "Name", ticket.AssignedGroup.Id);
            ViewData["CompanyId"] = new SelectList(_context.Company, "Id", "Name", ticket.Company.Id);
            return View(ticket);
        }


        public async Task<IActionResult> Reply()
        {
            return View();
        }

        public async Task<IActionResult> Close(int? id)
        {            
            if (ModelState.IsValid)
            {
                var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.Id == id);
                ticket.State = TicketState.Closed;

                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View();
        }
        
        public async Task<IActionResult> Assign()
        {
            return View();
        }
        
        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
