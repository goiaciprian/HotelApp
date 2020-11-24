using Backend.Dao;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Backend.Controllers
{
    public class ReservationsController : ApiController
    {
        // GET: api/Reservations
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var role = identity.Claims.Where(r => r.Type == ClaimTypes.Role).Select(r => r.Value).First();
            var id = identity.Claims.FirstOrDefault(uId => uId.Type == "Id").Value;
            if (role == "Admin")
                return Ok(await RezervariDao.getAllRezervationsAsync());
            else if (role == "User")
                return Ok(await RezervariDao.findAllReservationsOfOneUser(int.Parse(id)));
            else return BadRequest();
        }

        // GET: api/Reservations/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var found = await RezervariDao.findReservationById(id);
            if (found == null)
                return NotFound();
            else
                return Ok(found);
        }

        [HttpGet]
        [Route("api/reservations/allbyoneuser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetReservationsForOneUser(int userId)
        {
            var found = await RezervariDao.findAllReservationsOfOneUser(userId);
            if (!found.Any())
                return NotFound();
            else return Ok(found);
        }

        // POST: api/Reservations
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Post([FromBody] Rezervare rezervare)
        {
            if (rezervare.userId == 0)
                return BadRequest("User id is 0 or is not provided");
            if (rezervare.cameraId == 0)
                return BadRequest("Room id is 0 or is not provided");
            if (rezervare.nrCamere == 0)
                return BadRequest("Number of rooms is 0 or is not provided");
            if (rezervare.nrNopti == 0)
                return BadRequest("Number of nights is 0 or is not provided");
            if (rezervare.nrPersoane == 0)
                return BadRequest("Number of persons is 0 or is not provided");
            if (rezervare.rezervatPe.Minute == rezervare.rezervatPana.Minute)
                rezervare.setDefaultDate();

            if (await RezervariDao.addNewRezervationAsync(rezervare) == 1)
                return Content(HttpStatusCode.Created, "Reservation created");
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }

        [HttpDelete]
        [Route("api/reservations/deleteallbyuserid/{userid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteAllByUserId(int userid)
        {
            var code = await RezervariDao.deleteReservationsByUserId(userid);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");

        }


        // DELETE: api/Reservations/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var code = await RezervariDao.deleteReservationById(id);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }
    }
}
