using System.Data;
using Microsoft.AspNetCore.Authorization;
using Syriatel_Cafe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}