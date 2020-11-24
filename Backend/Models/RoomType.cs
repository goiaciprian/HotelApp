using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class RoomType
    {   
        [Key]
        public int id { get; set; } = 0;
        [Required]
        public string tipCamera { get; set; }
        [Required]
        public int camereDisponibile { get; set; }
        [Required]
        public int pretPeNoapte { get; set; }

        public override string ToString()
        {
            return $"{id} {tipCamera} {camereDisponibile} {pretPeNoapte}";
        }
    }
}