using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<PromoCode> PromoCode { get; set; }
        public DataContext() {  }

        public DataContext(DbContextOptions<DataContext> options) 
            : base(options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerPreference>()
               .HasKey(item => new { item.CustomerId, item.PreferenceId });

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(item => item.Customer)
                .WithMany(item => item.Preferences)
                .HasForeignKey(item => item.CustomerId);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(item => item.Preference)
                .WithMany()
                .HasForeignKey(item => item.PreferenceId);           

            modelBuilder.Entity<Customer>()
                .Property(item => item.FirstName)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<Customer>()
                .Property(item => item.LastName)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<Customer>()
                .Property(item => item.Email)
                .HasMaxLength(Constants.MaxLength);

            modelBuilder.Entity<Employee>()
                .Property(item => item.FirstName)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<Employee>()
                .Property(item => item.LastName)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<Employee>()
                .Property(item => item.Email)
                .HasMaxLength(Constants.MaxLength);

            modelBuilder.Entity<Role>()
                .Property(item => item.Name)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<Role>()
                .Property(item => item.Description)
                .HasMaxLength(Constants.MaxLength);

            modelBuilder.Entity<Preference>()
                .Property(item => item.Name)
                .HasMaxLength(Constants.MaxLength);

            modelBuilder.Entity<PromoCode>()
                .Property(item => item.PartnerName)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<PromoCode>()
                .Property(item => item.Code)
                .HasMaxLength(Constants.MaxLength);
            modelBuilder.Entity<PromoCode>()
                .Property(item => item.ServiceInfo)
                .HasMaxLength(Constants.MaxLength);
        }
    }
}
