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
    public class CategoriesController : ControllerBase
    {
        private readonly CafeModel db;
        public CategoriesController(CafeModel cafeModel)
        {
            db = cafeModel;
        }
        //private CafeModel db = new CafeModel();

        // GET: api/Categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            SystemStatus systemEnable = db.SystemStatus.FirstOrDefault();

            //if (systemEnable.SystemEnable == true)
            if (systemEnable is not null && systemEnable.SystemEnable)
            {
                var result = db.Categories.Select(x => new Category
                {
                    Enable = x.Enable,
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    Products = x.Products.Select(p => new Product
                    {
                        CategoryId = p.CategoryId,
                        AvailableProduact = p.AvailableProduact,
                        Enable = p.Enable,
                        Id = p.Id,
                        InitialPrice = p.InitialPrice,
                        IsDeleted = p.IsDeleted,
                        Name = p.Name
                    }).ToList()
                });
                return Ok(result);
            }

            else
            {
                return Ok(db.Categories.Where(x => x.Id == -1));

            }

        }
        [Route("/api/AdminGategories")]
        [HttpGet]

        [Authorize(Roles = "MIS-Technical Performance Information Analysis,AD-Catering Cashier,AD-Catering Staff")]
        public IQueryable<Category> AdminGategories()
        {


            return db.Categories.Include(x => x.Products);



        }


        //// GET: api/Categories/5
        //[ResponseType(typeof(Category))]
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        //[ResponseType(typeof(void))]
        [HttpPut("{id}")]
        public IActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                var ucategory = db.Categories.SingleOrDefault(x => x.Id == id);
                return Ok(ucategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Categories
        //[ResponseType(typeof(Category))]
        [HttpPost]
        public IActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return Ok(category);

        }

        // DELETE: api/Categories/5
        //[ResponseType(typeof(Category))]
        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}