using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_1._2.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; } = null!;

        public ICollection<AddressInUser> AddressInUsers { get; set; } = null!;
    }
}
