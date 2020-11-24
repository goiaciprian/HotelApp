using Backend.Dao;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Backend.Controllers
{
    public class RoomsController : ApiController
    {
        // GET: api/Rooms
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await RoomDao.findAllRoomTypesAsync());
        }

        // GET: api/Rooms/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var found = await RoomDao.findRoomTypeByIdAsync(id);
            if (found == null)
                return NotFound();
            else
                return Ok(found);
        }

        // POST: api/Rooms
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody]RoomType room)
        {
            try
            {
                if (room.pretPeNoapte == 0)
                    return BadRequest("The price cannot be 0");
                if (room.tipCamera == "")
                    return BadRequest("The room type cannot be blank");
                if(room.camereDisponibile == 0)
                    return BadRequest("The room's number cannot be 0");

                if (await RoomDao.addRoomTypeAsync(room) == 1)
                    return Content(HttpStatusCode.Created, "Room type created");
                else
                    return Content(HttpStatusCode.InternalServerError, "Error occured");
            }
            catch(NullReferenceException e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/Rooms/5
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody]RoomType update)
        {
            if (update.pretPeNoapte == 0)
                return BadRequest("The price cannot be 0");
            if (update.tipCamera == "")
                return BadRequest("The room type cannot be blank");
            if (update.camereDisponibile == 0)
                return BadRequest("The room's number cannot be 0");

            var code = await RoomDao.updateRoomTypeAsync(id, update);
            if (code == 1)
                return Ok("Room type updated");
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }

        // DELETE: api/Rooms/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var code = await RoomDao.deleteRoomTypeAsync(id);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }
    }
}
