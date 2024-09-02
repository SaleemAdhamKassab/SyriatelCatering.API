using System.Data;
using Microsoft.AspNetCore.Authorization;
using Syriatel_Cafe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyriatelCatering.API.Models;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly CafeModel db;
        public NewsController(CafeModel cafeModel)
        {
            db = cafeModel;
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            return Ok(db.News.Where(e => e.flag).ToList());
        }


        [HttpPut]
        [Authorize(Roles = "MIS-Technical Data Analysis,AD-Catering Cashier,AD-Catering Staff,AD-Catering HOSs")]
        public IActionResult PutNews([FromQuery]int id, [FromBody]New model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            db.Entry(model).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(model);
            

        }
    }
}