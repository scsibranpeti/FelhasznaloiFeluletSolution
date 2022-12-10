using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FelhasznaloiFelulet.Models
{
    public class Bank
    {
        [Key]
        public string Swift { get; set; }
        [DisplayName("Teljes név")]
        public string? Name { get; set; }
        [DisplayName("Bank székhelye")]
        public string? SeatAddress { get; set; }
        
    }
}
