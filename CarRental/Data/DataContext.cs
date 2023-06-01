using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):
            base(options)
        {  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // working
            modelBuilder.Entity<VehicleCategory>()
                .HasKey(pc => new { pc.VehicleId, pc.CategoryId });

            modelBuilder.Entity<VehicleCategory>() //Navigation property ( face legatura intre vehicle si category)
                .HasOne(p => p.Vehicle)
                .WithMany(pc => pc.VehicleCategories)
                .HasForeignKey(p => p.VehicleId);

            modelBuilder.Entity<VehicleCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.VehicleCategories)
                .HasForeignKey(c => c.CategoryId);
            //testing 
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.Reviews)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

 
        }

       

    public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
