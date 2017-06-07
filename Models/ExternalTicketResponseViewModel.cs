using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketer.Models
{
    public class ExternalTicketResponseViewModel
    {
        [Required]
        public string Token { get; set; }
        public string Content { get; set; }
    }
}
