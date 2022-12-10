using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FelhasznaloiFelulet.Models
{
    public class Address
    {
        [Key]
        [DisplayName("Lakcím azonosító")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayName("Város")]
        public string? City { get; set; }
        [DisplayName("Utca és házszám")]
        public string? Number { get; set; }
        [ForeignKey("CountryID")]
        public string? CountryID { get; set; }
        public Countries? Country { get; set; }

    }
}
