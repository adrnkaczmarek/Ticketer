using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketer.Database
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
    }
}
