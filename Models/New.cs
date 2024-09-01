using System.ComponentModel.DataAnnotations;

namespace SyriatelCatering.API.Models
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool flag { get; set; }
    }
}
