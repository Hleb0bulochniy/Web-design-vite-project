using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_1._2.Models
{
    public class Address
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("street")]
        public string Street { get; set; }
        [Column("house")]
        public string House { get; set; }

        public ICollection<AddressInUser> AddressInUsers { get; set; } = null!;
    }
}
