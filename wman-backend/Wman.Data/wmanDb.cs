using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wman.Data.DB_Models;

namespace Wman.Data
{
    public class wmanDb : IdentityDbContext<WmanUser, IdentityRole<int>, int,
   IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
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

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = 2, Name = "Manager", NormalizedName = "MANAGER" },
                new { Id = 3, Name = "Worker", NormalizedName = "WORKER" }
            );
        }
        public virtual DbSet<WorkEvent> WorkEvent { get; set; }
        public virtual DbSet<Label> Label { get; set; }
        public virtual DbSet<AddressHUN> Address { get; set; }
        public virtual DbSet<Pictures> Picture { get; set; }
        public virtual DbSet<ProofOfWork> ProofOfWork { get; set; }
    }
}
