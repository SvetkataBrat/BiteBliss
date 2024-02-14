using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BiteBlissDBContext : IdentityDbContext<User>
    {

        public BiteBlissDBContext() : base() { }

        public BiteBlissDBContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=TIMI-PC;Database=BiteBliss;Trusted_Connection=True;Encrypt=False");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole("Administrator") { NormalizedName = "ADMINISTRATOR" },
                new IdentityRole("User") { NormalizedName = "USER" });

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser("Admin") { NormalizedUserName = "ADMIN" });
        }

        //public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Recipe> Recipies { get; set; }

    }
}
