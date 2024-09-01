using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Product : ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]

        public string? Name { get; set; }

        [Required]
        public decimal InitialPrice { get; set; } = 0;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public bool Enable { get; set; } = true;

        public int AvailableProduact { get; set; }

        public virtual Category? Category { get; set; }


        public virtual ICollection<Transaction>? Transactions { get; set; }
        public bool IsDeleted { get; set; }



    }
}