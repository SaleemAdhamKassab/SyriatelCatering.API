using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syriatel_Cafe.Models;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExtrasController : ControllerBase
    {
        private readonly CafeModel db;
        public ExtrasController(CafeModel cafeModel)
        {
            db = cafeModel;
        }
        //private CafeModel db = new CafeModel();

        // GET: api/Extras
        [HttpGet]
        public IQueryable<Extra> GetExtras()
        {
            return db.Extras;
        }

        // GET: api/Extras/5
        //[ResponseType(typeof(Extra))]
        [HttpGet("{id}")]
        public IActionResult GetExtra(int id)
        {
            Extra extra = db.Extras.Find(id);
            if (extra == null)
            {
                return NotFound();
            }

            return Ok(extra);
        }

        // PUT: api/Extras/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        public IActionResult PutExtra(int id, Extra extra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != extra.Id)
            {
                return BadRequest();
            }

            db.Entry(extra).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                var uextra = db.Extras.SingleOrDefault(x => x.Id == id);

                return Ok(uextra);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExtraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Extras
        //[ResponseType(typeof(Extra))]
        [HttpPost]
        public IActionResult PostExtra(Extra extra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Extras.Add(extra);
            db.SaveChanges();
            return Ok(extra);

        }

        // DELETE: api/Extras/5
        //[ResponseType(typeof(Extra))]
        [HttpDelete("{id}")]
        public IActionResult DeleteExtra(int id)
        {
            Extra extra = db.Extras.Find(id);
            if (extra == null)
            {
                return NotFound();
            }

            db.Extras.Remove(extra);
            db.SaveChanges();

            return Ok(extra);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool ExtraExists(int id)
        {
            return db.Extras.Count(e => e.Id == id) > 0;
        }
    }
}