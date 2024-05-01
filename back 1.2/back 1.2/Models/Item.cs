using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_1._2.Models
{
    public class Item
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public string Price { get; set; } = null!;
        [Column("image")]
        public string Image { get; set; }

        public ICollection<ItemsInUser> ItemsInUser { get; set; } = null!;
    }
}
