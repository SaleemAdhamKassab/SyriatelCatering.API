using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Status : ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [ForeignKey("NextStatus")]
        public int? NextStatusId { get; set; }


        public virtual ICollection<Order>? Order { get; set; }

        public virtual Status? NextStatus { get; set; }
        public bool IsDeleted { get; set; }
    }
}