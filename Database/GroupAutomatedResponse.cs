using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketer.Database
{
    public class GroupAutomatedResponse
    {
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        [ForeignKey(nameof(AutomatedResponse))]
        public int AutomatedResponseId { get; set; }

        public virtual Group Group { get; set; }
        public virtual AutomatedResponse AutomatedResponse { get; set; }
    }
}
