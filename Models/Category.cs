using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class Category : ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool Enable { get; set; } = true;

        public virtual ICollection<Product> Products { get; set; }

        public bool IsDeleted { get; set; }
    }
}