using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class ServiciiPerUser
    {
        [Key]
        public int id { get; set; } = 0;
        [Required]
        public int userId { get; set; }
        [Required]
        public int serviciuId { get; set; }

        public string userName { get; set; } = "";

        public string numeServiciu { get; set; } = "";

        public int pretServiciu { get; set; } = 0;

        public override string ToString()
        {
            return $"{id} {userId} {serviciuId} {userName} {numeServiciu} {pretServiciu}";
        }
    }
}