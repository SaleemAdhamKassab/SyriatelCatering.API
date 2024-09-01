using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Extra: ISoftDelete
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; } = 0;

        public virtual ICollection<Transaction> Transactions { get; set; }
        public bool IsDeleted { get; set ; }
    }
}