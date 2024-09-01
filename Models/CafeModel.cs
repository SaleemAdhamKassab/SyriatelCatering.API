using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Syriatel_Cafe.Models;
using SyriatelCatering.API.Models;

namespace Syriatel_Cafe
{
    public class CafeModel : DbContext
    {
        public CafeModel(DbContextOptions<CafeModel> options) : base(options) { }

        // Your context has been configured to use a 'CafeModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Syriatel_Cafe.CafeModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CafeModel' 
        // connection string in the application configuration file.

        //public CafeModel(): base("name=CafeModel")
        //{
        //    Configuration.ProxyCreationEnabled = false;
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<CafeModel, Configuration>());
        //}


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Filter("IsDeleted", (ISoftDelete d) => d.IsDeleted, false);
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Extra> Extras { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<SystemStatus> SystemStatus { get; set; }
        public DbSet<New> News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                    .HasIndex(u => new { u.Name, u.CategoryId })
                    .IsUnique();

            modelBuilder.Entity<Status>()
                  .HasIndex(u => u.Name)
                  .IsUnique();


            modelBuilder.Entity<Category>()
                  .HasIndex(u => u.Name)
                  .IsUnique();

            modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Extra>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Status>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);


            base.OnModelCreating(modelBuilder);
        }
    }
}