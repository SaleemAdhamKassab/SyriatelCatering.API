using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Transaction: ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]

        public int OrdertId { get; set; }

        [Required]
        [ForeignKey("Product")]

        public int ProductId { get; set; }

        public int Quantity { get; set; }


        public decimal UnitPrice { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }



        public virtual ICollection<Extra>? Extras { get; set; }
        public bool IsDeleted { get; set; }
    }
}