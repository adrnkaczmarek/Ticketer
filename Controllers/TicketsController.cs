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
using Ticketer.Models;
using Ticketer.Tokens;

namespace PutNet.Web.Identity.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketContext _context;
        private readonly UserManager<User> _userManager;
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private readonly ITokenResolver _tokenResolver;

        public TicketsController(UserManager<User> userManager, TicketContext context, ITokenResolver tokenResolver)
        {
            _context = context;
            _userManager = userManager;
            _tokenResolver = tokenResolver;
        }
        
        public async Task<IActionResult> Index(TicketFilters? filter)
        {
            User currentUser = await GetCurrentUserAsync();
            bool globalFiltering = false;
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
                        globalFiltering = true;
                        tickets = tickets.Where(ticket => ticket.Assigned == null).ToList();
                        break;
                    case TicketFilters.MyGroup:
                        globalFiltering = true;

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

            if (!globalFiltering)
            {
                tickets = tickets.Where(ticket => ticket.AssignedId == currentUser.Id).ToList();
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

        public async Task<IActionResult> DeleteConfirm(int? id)
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

            ViewData["Id"] = id;
            return PartialView("_DeleteConfirm");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitTicketViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            Source source;
            try
            {
                source = await _tokenResolver.ResolveSourceToken(model.Token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // Should be only one always
            var routing = source.SourceRoutings.FirstOrDefault();
            if (routing == null)
            {
                return BadRequest();
            }

            var ticket = new Ticket
            {
                CompanyId = source.CompanyId,
                AssignedGroupId = routing.GroupId,
                CreatedAt = DateTime.Now,
                Priority = model.Priority,
                Title = model.Title,
                State = TicketState.Open,
                ExternalTicketResponses = new List<ExternalTicketResponse>()
            };

            var response = new ExternalTicketResponse
            {
                Content = model.Content,
                ExternalClient = new ExternalClient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone
                },
                Timestamp = DateTime.Now
            };

            ticket.ExternalTicketResponses.Add(response);
            _context.Tickets.Add(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            //TODO: Return token for ticket
            return Content("");
        }
        
        public async Task<IActionResult> Reply(int id)
        {
            ViewData["ticketId"] = id;
            return PartialView("_Reply");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply([Bind("Id,TicketId,Content")] TicketResponse response)
        {
            return PartialView("_Success");
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
        
        public async Task<IActionResult> AssignTicketTo(int Id)
        {
            User currentUser = await GetCurrentUserAsync();
            var users = await _context.Users.ToArrayAsync();
            ViewData["TicketId"] = Id;
            return PartialView("_AssignTicketTo", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTicketTo(int ticketId, string userId)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.Id == ticketId);
            var userToAssign = await _context.Users.Where(user => user.Id == userId).SingleOrDefaultAsync();
            ticket.Assigned = userToAssign;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
