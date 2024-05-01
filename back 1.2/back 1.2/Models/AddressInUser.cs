using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_1._2.Models
{
    public class AddressInUser
    {
        [Key]
        public int Id { get; set; }

        public Address Address { get; set; } = null!;
        public int AddressId { get; set; }

        public User User { get; set; } = null!;
        public int UserId { get; set; }


    }
}
