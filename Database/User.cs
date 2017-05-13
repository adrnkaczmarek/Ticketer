using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ticketer.Database
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Group UserGroup { get; set; }
    }
}