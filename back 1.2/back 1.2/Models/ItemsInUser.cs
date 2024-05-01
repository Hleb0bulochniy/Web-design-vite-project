using System.ComponentModel.DataAnnotations;

namespace back_1._2.Models
{
    public class ItemsInUser
    {
        [Key]
        public int Id { get; set; }

        public Item item { get; set; } = null!;
        public int itemId { get; set; }
        public bool isFavourite { get; set; }

        public User User { get; set; } = null!;
        public int UserId { get; set; }
    }
}
