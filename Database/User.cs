using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ticketer.Database
{
    public class User : IdentityUser
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<AutomatedResponse> CreatedAutomatedResponses { get; set; }
    }
}