using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class Serviciu
    {
        [Key]
        public int id { get; set; } = 0;
        [Required]
        public string numeServiciu { get; set; } = "";
        [Required]
        public int pret { get; set; } = 0;

        public override string ToString()
        {
            return $"{id} {numeServiciu} {pret}";
        }
    }
}