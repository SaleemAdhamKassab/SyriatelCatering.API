using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Order: ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string  UserName { get; set; }

        [NotMapped]
        public decimal Total { get; set; } = 0;
        public DateTime CreateDate { get; set; } = DateTime.Now;


        [ForeignKey("Status")]
        public int? StatusId { get; set; } = 2;

        public string Location { get; set; }

        public int Roof { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Status Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}