using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syriatel_Cafe.Models;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private CafeModel db = new CafeModel();
        private readonly CafeModel db;
        public ProductsController(CafeModel cafeModel)
        {
            db = cafeModel;
        }

        // GET: api/Products
        [HttpGet]
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        //[ResponseType(typeof(Product))]
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        [Authorize(Roles = "MIS-Technical Data Analysis,AD-Catering Cashier,AD-Catering Staff,AD-Catering HOSs")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                var uproduct = db.Products.Find(id);

                return Ok(uproduct);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/Products
        //[ResponseType(typeof(Product))]
        [HttpPost]
        [Authorize(Roles = "MIS-Technical Data Analysis,AD-Catering Cashier,AD-Catering Staff,AD-Catering HOSs")]
        public IActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return Ok(product);

        }

        // DELETE: api/Products/5
        //[ResponseType(typeof(Product))]
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}