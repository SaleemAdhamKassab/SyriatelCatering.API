using System.Data;
using System.Net;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syriatel_Cafe.Models;
using SyriatelCatering.API.Models.DTOs;

namespace Syriatel_Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        //private CafeModel db = new CafeModel();
        private readonly CafeModel db;
        public OrdersController(CafeModel cafeModel)
        {
            db = cafeModel;
            timeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
        }
        private TimeZoneInfo timeZone;
        // GET: api/Orders
        [HttpPost("getorders")]
        public IActionResult GetOrders([FromBody] GetOrderFilter filter)
        {
            filter.Date = TimeZoneInfo.ConvertTimeFromUtc(filter.Date, timeZone);
            var result = db.Orders.Include(x => x.Status).Include(x => x.User).Where(x => !x.IsDeleted && x.CreateDate >= filter.Date && x.CreateDate < filter.Date.AddDays(1)).ToList();
            return Ok(result);
        }

        // GET: api/Orders/5
        //[ResponseType(typeof(Order))]
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = db.Orders.Include(x => x.Transactions).Where(x => x.Id == id).ToList();
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        //[ResponseType(typeof(void))]
        [HttpPut]
        public IActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return NotFound();
        }

        // POST: api/Orders
        //[ResponseType(typeof(Order))]
        [HttpPost]
        public IActionResult PostOrder(Order order)
        {
            //WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var identity = User.Identity.Name;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<string> checkResult = CheckAvailable(order);

            if (checkResult.Count == 0)
            {

                foreach (var t in order.Transactions)

                {

                    var lstExtra = t.Extras.ToList();
                    t.Extras.Clear();


                    foreach (var e in lstExtra)
                    {
                        t.Extras.Add(db.Extras.Find(e.Id));

                    }
                    var product = db.Products.Find(t.ProductId);

                    var Aquantity = product.AvailableProduact;


                    Aquantity = Aquantity - t.Quantity;


                    product.AvailableProduact = Aquantity;

                    db.Entry(product).State = EntityState.Modified;

                    db.SaveChanges();


                }

                //order.UserName = identity.Name.Substring(9);
                order.UserName = identity.Substring(9);

                db.Orders.Add(order);
                db.SaveChanges();

                return Ok(order);
            }
            else
            {
                return Ok(checkResult);
            }
        }


        // DELETE: api/Orders/5
        //[ResponseType(typeof(Order))]
        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }


        [HttpGet]
        [Route("/api/HandelOrder")]
        public IActionResult HandelTicket(int orderid)
        {
            Order order = db.Orders.Find(orderid);

            var nextStatus = db.Status.Where(x => x.Id == order.StatusId).Select(x => x.NextStatusId).Single();

            if (nextStatus != null)
            {
                order.StatusId = Convert.ToInt32(nextStatus);

                db.Entry(order).State = EntityState.Modified;

                db.SaveChanges();

                return Ok(true);

            }

            else
            {
                return Ok(false);
            }




        }

        [HttpGet]
        [Route("/api/RejectOrder")]
        public IActionResult RejectOrder(int orderid)
        {
            var order = db.Orders.Include(x => x.Transactions).FirstOrDefault(y => y.Id == orderid);


            if (order != null)
            {
                order.StatusId = 4;
                db.Entry(order).State = EntityState.Modified;

                db.SaveChanges();


                foreach (var s in order.Transactions)
                {

                    var product = db.Products.Find(s.ProductId);

                    int reloadProductAvailable = product.AvailableProduact + s.Quantity;

                    product.AvailableProduact = reloadProductAvailable;

                    db.Entry(product).State = EntityState.Modified;

                    db.SaveChanges();

                }

                return Ok(true);

            }
            else
            {
                return Ok(false);
            }


        }


        private List<string> CheckAvailable(Order t)
        {
            List<string> NoAvailable = new List<string>();

            string msg = "";

            foreach (Transaction s in t.Transactions)
            {

                var product = db.Products.Find(s.ProductId);

                var Aquantity = product.AvailableProduact;


                if (Convert.ToInt32(Aquantity) < s.Quantity)
                {
                    msg = product.Name;

                    NoAvailable.Add(msg);

                }


            }

            return NoAvailable;




        }


        [HttpGet]
        [Route("/api/OrderUser")]
        public IActionResult GetUserOrder([FromQuery]DateTime dateFilter)
        {
            //WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var identity = User.Identity.Name;


            //string d = db.Orders.Select(x => EntityFunctions.TruncateTime(x.CreateDate)).ToString();

            ////var orders = db.Orders.Where(x => x.UserName == identity.Name.Substring(9) && EntityFunctions.TruncateTime(x.CreateDate) == EntityFunctions.TruncateTime(dateFilter)).Include(x => x.Status);
            //var orders = db.Orders.Where(x => x.UserName == identity.Substring(9) && EntityFunctions.TruncateTime(x.CreateDate) == EntityFunctions.TruncateTime(dateFilter)).Include(x => x.Status);

            //return Ok(orders);

            dateFilter = TimeZoneInfo.ConvertTimeFromUtc(dateFilter, timeZone);
            var result = db.Orders.Include(x => x.Status).Where(x => x.UserName == identity.Substring(9) && !x.IsDeleted && x.CreateDate >= dateFilter && x.CreateDate < dateFilter.AddDays(1)).ToList();
            return Ok(result);



        }

        [HttpGet]
        [Route("/api/orderTransaction")]
        public IActionResult GetTransaction(int orderid)
        {
            //WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var identity = User.Identity.Name;


            var transaction = db.Transactions.Where(x => x.OrdertId == orderid).Include(y => y.Product).Include(z => z.Extras);

            return Ok(transaction);


        }
    }
}