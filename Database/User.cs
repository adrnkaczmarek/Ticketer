using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ticketer.Database
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group UserGroup { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<AutomatedResponse> CreatedAutomatedResponses { get; set; }
    }
}