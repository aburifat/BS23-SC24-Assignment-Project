using System.ComponentModel.DataAnnotations.Schema;

namespace BS23_SC24_Assignment_Backend.Models
{
    [Table(nameof(UserRole))]
    public class UserRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
