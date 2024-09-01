using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syriatel_Cafe.Models;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        //private CafeModel db = new CafeModel();
        private readonly CafeModel db;
        public UsersController(CafeModel cafeModel)
        {
            db = cafeModel;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(db.Users.ToList());
        }

        // GET: api/Users/5
        //[ResponseType(typeof(User))]
        [HttpGet("getMyUser")]
        public IActionResult GetUser( [FromQuery] string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Ok(false);
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        public IActionResult PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserName)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                var uuser = db.Users.SingleOrDefault(x => x.UserName == id);

                return Ok(uuser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Users
        //[ResponseType(typeof(User))]
        [HttpPost]
        public IActionResult PostUser(User user)
        {
            //WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var identity = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.UserName = identity.Substring(9);
            user.CreateDate = DateTime.Now;

            db.Users.Add(user);

            try
            {
                db.SaveChanges();

                return Ok(user);
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/Users/5
        //[ResponseType(typeof(User))]
        [HttpDelete]
        public IActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }

        [HttpGet]
        [Route("/api/CurrentUser")]
        public IActionResult GetCurrentUser()
        {
            //WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var identity = User.Identity.Name;

            //var GetCurrentUser = db.Users.Where(x=>x.UserName==identity.Name.Substring(9));

            var GetCurrentUser = identity.Substring(9);

            return Ok(GetCurrentUser);


        }
    }
}