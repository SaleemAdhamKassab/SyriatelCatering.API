using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syriatel_Cafe.Models;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemStatusController : ControllerBase
    {
        //private CafeModel db = new CafeModel();
        private readonly CafeModel db;
        public SystemStatusController(CafeModel cafeModel)
        {
            db = cafeModel;
        }

        // GET: api/SystemStatus
        //public IQueryable<SystemStatus> GetSystemStatus()
        //{
        //    return db.SystemStatus;
        //}
        [HttpGet]
        public IActionResult GetSystemStatus()
        {
            return Ok(db.SystemStatus.ToList());
        }

        // GET: api/SystemStatus/5
        //[ResponseType(typeof(SystemStatus))]
        [HttpGet("{id}")]
        public IActionResult GetSystemStatus(int id)
        {
            SystemStatus systemStatus = db.SystemStatus.Find(id);
            if (systemStatus == null)
            {
                return NotFound();
            }

            return Ok(systemStatus);
        }

        // PUT: api/SystemStatus/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        public IActionResult PutSystemStatus(int id, SystemStatus systemStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != systemStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(systemStatus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok();
        }

        // POST: api/SystemStatus
        //[ResponseType(typeof(SystemStatus))]
        [HttpPost]
        public IActionResult PostSystemStatus(SystemStatus systemStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SystemStatus.Add(systemStatus);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = systemStatus.Id }, systemStatus);
        }

        // DELETE: api/SystemStatus/5
        //[ResponseType(typeof(SystemStatus))]
        [HttpDelete]
        public IActionResult DeleteSystemStatus(int id)
        {
            SystemStatus systemStatus = db.SystemStatus.Find(id);
            if (systemStatus == null)
            {
                return NotFound();
            }

            db.SystemStatus.Remove(systemStatus);
            db.SaveChanges();

            return Ok(systemStatus);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool SystemStatusExists(int id)
        {
            return db.SystemStatus.Count(e => e.Id == id) > 0;
        }


        [HttpGet]
        [Authorize(Roles = "MIS-Technical Data Analysis,AD-Catering Cashier,AD-Catering Staff,AD-Catering HOSs")]
        [Route("/api/CloseSystem")]
        public bool CloseSystem()
        {
            var data = db.SystemStatus.FirstOrDefault();

            data.SystemEnable = false;

            db.Entry(data).State = EntityState.Modified;

            db.SaveChanges();
            return true;

        }

        [HttpGet]
        [Authorize(Roles = "MIS-Technical Data Analysis,AD-Catering Cashier,AD-Catering Staff,AD-Catering HOSs")]
        [Route("/api/OpenSystem")]
        public bool OpenSystem()
        {
            var data = db.SystemStatus.FirstOrDefault();

            data.SystemEnable = true;

            db.Entry(data).State = EntityState.Modified;

            db.SaveChanges();

            return true;

        }

    }
}