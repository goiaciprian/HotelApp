using Backend.Dao;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Backend.Controllers
{
    public class ServiciiController : ApiController
    {
        // GET: api/Servicii
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await ServiciiDao.findAllServiciiAsync());
        }

        // GET: api/Servicii/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var found = await ServiciiDao.findServiciuByIdAsync(id);
            if (found == null)
                return NotFound();
            else
                return Ok(found);
        }

        // POST: api/Servicii
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Post([FromBody] Serviciu serviciu)
        {
            try
            {
                if (serviciu.numeServiciu == "")
                    return BadRequest("The name cannot be blank");
                if (serviciu.pret == 0)
                    return BadRequest("The price cannot be 0");

                if (await ServiciiDao.addServiciuAsync(serviciu) == 1)
                    return Content(HttpStatusCode.Created, "Service created");
                else
                    return Content(HttpStatusCode.InternalServerError, "Error occured");
            }
            catch (NullReferenceException e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/Servicii/5
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Serviciu serviciu)
        {
            if (serviciu.numeServiciu == "")
                return BadRequest("The name cannot be blank or is not privided");
            if (serviciu.pret == 0)
                return BadRequest("The price cannot be 0 or is not provided");

            var code = await ServiciiDao.updateServiciuAsync(id, serviciu);
            if (code == 1)
                return Ok("Service updated");
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured.");

        }

        // DELETE: api/Servicii/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var code = await ServiciiDao.deleteServiciuAsync(id);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else return Content(HttpStatusCode.InternalServerError, "Error occured.");
        }
    }
}
