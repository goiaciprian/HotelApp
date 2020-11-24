using Backend.Dao;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Backend.Controllers
{
    public class UsersController : ApiController
    {

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles ="User, Admin")]
        public async Task<IHttpActionResult> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var role = identity.Claims.Where(r => r.Type == ClaimTypes.Role).Select(r => r.Value).First();
            if(role.Equals("Admin"))
                return Ok(await UserDao.findAllAsync());
            else
            {
                var IdString = identity.Claims.FirstOrDefault(id => id.Type == "Id");
                List<User> user = new List<User>();
                user.Add(await UserDao.findByIdAsync(int.Parse(IdString.Value)));
                return Ok(user);
            }
        }

        // GET: api/Users/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var found = await UserDao.findByIdAsync(id);
            if (found == null)
                return NotFound();
            else
                return Ok(found);

        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]User user)
        {
            try
            {
                if (user.email.Equals(""))
                    return BadRequest("Email cannot be blank;");
                if (user.fullName.Equals(""))
                    return BadRequest("The name cannot be blank;");
                if (user.password.Equals(""))
                    return BadRequest("The password cannot be blank;");

                var code = await UserDao.addUserAsync(user);
                if (code == 1)
                    return Content(HttpStatusCode.Created, "User created.");
                else if (code == 3)
                    return Content(HttpStatusCode.Found, "Email already in database.");
                else
                    return Content(HttpStatusCode.InternalServerError, "Error occured");
            } catch (NullReferenceException e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/Users/5
        [HttpPut]
        [Authorize(Roles = "User, Admin")]
        public async Task<IHttpActionResult> Put(int id, [FromBody]User update)
        {
            if (update.email.Equals(""))
                return BadRequest("Email cannot be blank;");
            if (update.fullName.Equals(""))
                return BadRequest("The name cannot be blank;");
            if (update.password.Equals(""))
                return BadRequest("The password cannot be blank;");

            var code = await UserDao.updateUserAsync(id, update);
            if (code == 1)
                return Ok("User updated");
            else if (code == 0)
                return NotFound();
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured.");

        }

        // DELETE: api/Users/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var code = await UserDao.deleteUserByIdAsync(id);
            if (code == 1)
                return Ok();
            else if (code == 0)
                return NotFound();
            else if (code == 2)
                return Content(HttpStatusCode.Conflict, "Cannot delete user because has reservations.");
            else
                return Content(HttpStatusCode.InternalServerError, "Error occured");
        }
    }
}
