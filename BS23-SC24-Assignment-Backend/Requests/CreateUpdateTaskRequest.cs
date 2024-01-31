using BS23_SC24_Assignment_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BS23_SC24_Assignment_Backend.Requests
{
    public class CreateUpdateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
