using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class Rezervare
    {
        [Key]
        public int id { get; set; } = 0;
        [Required]
        public int nrNopti { get; set; } = 0;
        [Required]
        public int nrPersoane { get; set; } = 0;
        [Required]
        public int nrCamere { get; set; } = 0;
        [Required]
        public int userId { get; set; } = 0;
        public string userName { get; set; } = "";
        [Required]
        public int cameraId { get; set; } = 0;
        public string tipCamera { get; set; } = "";
        public int pretPeNoapte { get; set; } = 0;
        public DateTime rezervatPe { get; set; } = DateTime.Now;
        [Required]
        public DateTime rezervatPana { get; set; } = DateTime.Now;
        public int pretFaraServicii { get; set; } = 0;
        public int totalPretServicii { get; set; } = 0;

        public void setDefaultDate()
        {
            rezervatPana = rezervatPe.AddDays(nrNopti);
        }

        public override string ToString()
        {
            return $"{id} {nrNopti} {nrPersoane} {nrCamere} {userId} {userName} {cameraId} {tipCamera} {pretPeNoapte} {rezervatPe} {rezervatPana} {pretFaraServicii} {totalPretServicii}";
        }
    }
}