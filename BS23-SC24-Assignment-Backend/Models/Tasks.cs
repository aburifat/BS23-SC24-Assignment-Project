using System.ComponentModel.DataAnnotations.Schema;

namespace BS23_SC24_Assignment_Backend.Models
{
    [Table(nameof(Tasks))]
    public class Tasks
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
