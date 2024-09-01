using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Syriatel_Cafe.Models
{
    public class User
    {
        [Key]
        public string? UserName { get; set; }
        
        [Phone]
        [Required]

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public int Floor { get; set; }

        public string? Note { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
    }
}