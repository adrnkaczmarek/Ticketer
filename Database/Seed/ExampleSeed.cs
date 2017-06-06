using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ticketer.Database.Enums;

namespace Ticketer.Database.Seed
{
    public class ExampleSeed
    {
        public async Task Seed(IServiceProvider provider)
        {
            var context = provider.GetService<TicketContext>();
            if (!await context.Database.EnsureCreatedAsync())
            {
                return;
            }

            var group1 = new Group
            {
                Name = "Tech Support"
            };

            var group2 = new Group
            {
                Name = "Common"
            };

            var company = new Company
            {
                Name = "Example Company",
                Groups = new List<Group> { group1, group2 }
            };

            context.Company.Add(company);
            await context.SaveChangesAsync();


            var userManager = provider.GetService<UserManager<User>>();
            var user1 = new User
            {
                FirstName = "John",
                LastName = "Concrete",
                Email = "john.conrete@example.com",
                UserName = "j.concrete",
                Group = group2
            };

            var user2 = new User
            {
                FirstName = "Sebastian",
                LastName = "Alex",
                Email = "sebastian.alex@example.com",
                UserName = "s.alex",
                Group = group1
            };

            var user3 = new User
            {
                FirstName = "Adrian",
                LastName = "Kaczmarek",
                Email = "adrian.kaczmarek@example.com",
                UserName = "a.kaczmarek",
                Group = group1
            };

            var user4 = new User
            {
                FirstName = "James",
                LastName = "Kirk",
                Email = "james.kirk@example.com",
                UserName = "kapitan",
                Group = group2
            };

            var results = new[]
            {
                await userManager.CreateAsync(user1, "1111"),
                await userManager.CreateAsync(user2, "1111"),
                await userManager.CreateAsync(user3, "1111"),
                await userManager.CreateAsync(user4, "1111")
            };

            var exampleResponse = new AutomatedResponse
            {
                Match = "help",
                Content = "Welcome to Example Company! Lorem ipsum Lala",
                Created = DateTime.Now,
                CreatedBy = user2,
                MaxPriority = TicketPriority.Normal,
                Modified = DateTime.Now,
                GroupAutomatedResponses = new List<GroupAutomatedResponse>()
            };
            var groupAutomatedResponse = new GroupAutomatedResponse
            {
                Group = group1,
                AutomatedResponse = exampleResponse
            };
            exampleResponse.GroupAutomatedResponses.Add(groupAutomatedResponse);
            context.AutomatedResponses.Add(exampleResponse);

            var source = new Source
            {
                Name = "ExampleSite",
                Website = "www.example.com",
                SourceRoutings = new List<SourceRouting>(),
                Company = company
            };
            var sourceRouting = new SourceRouting
            {
                Source = source,
                Group = group1,
                Created = DateTime.Now,
                CreatedBy = user3
            };
            source.SourceRoutings.Add(sourceRouting);
            context.Sources.Add(source);

            await context.SaveChangesAsync();
        }
    }
}
