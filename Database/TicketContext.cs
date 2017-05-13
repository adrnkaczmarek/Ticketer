using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ticketer.Database
{
    public class TicketContext : IdentityDbContext<User>
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options){ }

        public DbSet<Ticket> tickets { get; set; }
        public DbSet<Group> groups { get; set; }
    }
}