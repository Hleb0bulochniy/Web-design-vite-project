using back_1._2.Models;
using Microsoft.EntityFrameworkCore;


namespace back_1._2.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Address> Address { get; set; } = null!;
        public DbSet<AddressInUser> AddressInUsers { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<ItemsInUser> ItemsInUser { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=Backend12;Persist Security Info=True;User ID=daniil;Password=test;Trust Server Certificate=True");
        }
    }
}
