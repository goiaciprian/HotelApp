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
    public class ServiciiUsersController : ApiController
    {
        // GET: api/ServiciiUsers
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await ServiciiPerUserDao.getAllUsersServicesAsync());
        }

        // GET: api/ServiciiUsers/5
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var found = await ServiciiPerUserDao.getServiciuPerUserById(id);
            if (found == null)
                return NotFound();
            else return Ok(found);
        }
        [HttpGet]
        [Route("api/serviciiusers/allbyuser/{userid}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IHttpActionResult> GetByUserId(int userid)
        {
            var found = await ServiciiPerUserDao.getOneUserAllServicesAsync(userid);
            if (!found.Any())
                return NotFound();
            else return Ok(found);
        }

        // POST: api/ServiciiUsers
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] ServiciiPerUser serviciu)
        {
            if (serviciu.userId == 0)
                return BadRequest("User id cannot be empty");
            if (serviciu.serviciuId == 0)
                return BadRequest("Service id cannot be empty");

            var code = await ServiciiPerUserDao.addNewServiceToUser(serviciu);
            if (code == 1)
                return Content(HttpStatusCode.Created, "User service added");
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }

        // DELETE: api/ServiciiUsers/5
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var code = await ServiciiPerUserDao.deleteServicePerUserAsync(id);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");

        }
    }
}
