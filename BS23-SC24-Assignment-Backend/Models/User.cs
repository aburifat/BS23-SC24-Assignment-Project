﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BS23_SC24_Assignment_Backend.Models
{
    [Table(nameof(User))]
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long UserRoleId { get; set; }
        [ForeignKey(nameof(UserRoleId))]
        public virtual UserRole UserRole { get; set; }
    }
}
