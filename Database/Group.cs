using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketer.Database
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Company")]
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<SourceRouting> SourceRoutings { get; set; }
        public virtual ICollection<GroupAutomatedResponse> GroupAutomatedResponses { get; set; }
    }
}
