using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int id { get; set; } = 0;
        [Required]
        public string fullName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public int isAdmin { get; set; } = 0;

        public override string ToString()
        {
            return $"{id} {fullName} {email} {password} {isAdmin}";
        }
    }
}
