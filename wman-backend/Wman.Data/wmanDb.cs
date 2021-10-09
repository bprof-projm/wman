using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Connection_Tables;
using Wman.Data.DB_Models;

namespace Wman.Data
{
    public class wmanDb : IdentityDbContext<WmanUser, WmanRole, int,
   IdentityUserClaim<int>, WmanUserRole, IdentityUserLogin<int>,
   IdentityRoleClaim<int>, IdentityUserToken<int>>
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
                //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testwmandb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WmanUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

            modelBuilder.Entity<WmanRole>()
              .HasMany(ur => ur.UserRoles)
              .WithOne(u => u.Role)
              .HasForeignKey(ur => ur.RoleId)
              .IsRequired();

            modelBuilder.Entity<IdentityRole>().HasData(
                new { Id = "0d301757-99d2-4253-aac2-39e298dd0ab7", Name = "Debug", NormalizedName = "DEBUG" }
            modelBuilder.Entity<WorkEventPicture>()
                .HasKey(x => new { x.WorkEventId, x.PictureId });
            modelBuilder.Entity<WorkEventPicture>()
                .HasOne(x => x.WorkEvent)
                .WithMany(y => y.ProofOfWorkPic)
                .HasForeignKey(z => z.PictureId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WorkEventPicture>()
                .HasOne(x => x.Picture)
                .WithMany(y => y.WorkEvents)
                .HasForeignKey(z => z.WorkEventId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WmanRole>().HasData(
                new {Id= 1, Name = "Debug", NormalizedName = "DEBUG" }
            );
        }
        public virtual DbSet<DB_Models.WorkEvent> WorkEvent { get; set; }

    }
}
