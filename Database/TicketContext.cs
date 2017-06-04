using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ticketer.Database;

namespace Ticketer.Database
{
    public class TicketContext : IdentityDbContext<User>
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options){ }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ExternalClient> ExternalClients { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceRouting> SourceRoutings { get; set; }
        public DbSet<AutomatedResponse> AutomatedResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Source>()
                .HasMany(s => s.SourceRoutings)
                .WithOne(sr => sr.Source)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Group>()
                .HasMany(g => g.SourceRoutings)
                .WithOne(sr => sr.Group)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SourceRouting>()
                .HasKey(sr => new {sr.SourceId, sr.GroupId});

            builder.Entity<Group>()
                .HasMany(g => g.GroupAutomatedResponses)
                .WithOne(gr => gr.Group)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AutomatedResponse>()
                .HasMany(a => a.GroupAutomatedResponses)
                .WithOne(ar => ar.AutomatedResponse)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GroupAutomatedResponse>()
                .HasKey(ar => new {ar.GroupId, ar.AutomatedResponseId});

            builder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithOne(u => u.Group);

            builder.Entity<Company>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Company)
                .OnDelete(DeleteBehavior.Restrict);
                

            base.OnModelCreating(builder);
        }

        public DbSet<Ticketer.Database.User> User { get; set; }
    }
}