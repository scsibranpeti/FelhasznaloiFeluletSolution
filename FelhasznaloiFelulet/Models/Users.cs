using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FelhasznaloiFelulet.Models
{
    public class Users
    {
        [Key]
        [DisplayName("Azonosító")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayName("Vezetéknév")]
        public string Lastname { get; set; }
        [DisplayName("Keresztnév")]
        public string Firstname { get; set; }
        [DisplayName("Telefonszám")]
        public int Mobile { get; set; }
        [DisplayName("Számlaszám")]
        public int AccountNumber { get; set; }
        [ForeignKey("Bank")]
        public string? BankSwift { get; set; }
        [Required]
        public Address? UserAddress { get; set; }
        public Bank? UserBank { get; set; }
    }
}
