using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ticketer.Database
{
    public class SourceRouting
    {
        public DateTime Created { get; set; } = DateTime.Now;
        [ForeignKey(nameof(CreatedBy)), Required]
        public string CreatedById { get; set; }

        [ForeignKey(nameof(Source))]
        public int SourceId { get; set; }
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
    
        public virtual Source Source { get; set; }
        public virtual Group Group { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
