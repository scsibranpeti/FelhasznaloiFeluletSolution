using FelhasznaloiFelulet.Models;
using Microsoft.EntityFrameworkCore;

namespace FelhasznaloiFelulet.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Address> Address { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Users> Users { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Users>()
        //        .HasOne(u => u.UserAddress)
        //        .WithOne()
        //        .HasForeignKey<Address>(a => a.UserID);
        //}
    }
}
