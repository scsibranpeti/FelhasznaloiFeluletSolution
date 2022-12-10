using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FelhasznaloiFelulet.Models
{
    public class Countries
    {
        [Key]
        [DisplayName("Ország betűjele")]
        public string ID { get; set; }
        [DisplayName("Ország neve")]
        public string? Name { get; set; }
        [DisplayName("EU-tagállam")]
        public Boolean? isEU { get; set; }
        [DisplayName("Ország hívószáma")]
        public int? CountryTel { get; set; }
        
    }
}
