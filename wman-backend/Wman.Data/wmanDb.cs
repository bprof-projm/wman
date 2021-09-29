using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Data
{
    public class wmanDb : IdentityDbContext<IdentityUser>
    {
        public wmanDb()
        {
            this.Database.EnsureCreated();
        }

        public wmanDb(DbContextOptions<wmanDb> opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\wmandb.mdf;integrated security=True;MultipleActiveResultSets=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityRole>().HasData(
                new { Id = "0d301757-99d2-4253-aac2-39e298dd0ab7", Name = "Debug", NormalizedName = "DEBUG" }
            );
        }
        public virtual DbSet<DB_Models.WorkEvent> WorkEvent { get; set; }

    }
}
