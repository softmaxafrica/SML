using LogiSync.Models;
using lsclient.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace lsclient.Server.Shared
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        #region DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TruckType> TruckTypes { get; set; } 
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<RequestWithPayment> PriceAgreements { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ChargableItem> ChargableItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet <SecUser> SecUsers { get;set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<GenTranslation> GenTranslations { get; set; }
        public DbSet<GenTable> GenTables { get; set; }
        public DbSet<GenTableColumn> GenTableColumns { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Image> Images { get; set; }

        #endregion

        #region AppDbContext

        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DB_Connection"));
            }
            base.OnConfiguring(optionsBuilder);
        }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Company and Truck
            //modelBuilder.Entity<Truck>()
            //    .HasOne(t => t.Company)
            //    .WithMany(c => c.Trucks)
            //    .HasForeignKey(t => t.CompanyID)
            //    .OnDelete(DeleteBehavior.Cascade);

            // JobRequest and PriceAgreement
            modelBuilder.Entity<JobRequest>()
            .HasOne(jr => jr.PriceAgreement)
              .WithOne()
              .HasForeignKey<JobRequest>(jr => jr.PriceAgreementID)
                 .OnDelete(DeleteBehavior.Cascade);


            // Invoice and JobRequest
            //modelBuilder.Entity<Invoice>()
            //     .HasOne(i => i.JobRequest)
            //     .WithMany(jr => jr.Invoices)
            //     .HasForeignKey(i => i.JobRequestID)
            //     .OnDelete(DeleteBehavior.Cascade);
 

            // JobRequest and Customer
            //modelBuilder.Entity<JobRequest>()
            //    .HasOne(jr => jr.Customer)
            //    .WithMany(c => c.JobRequests)
            //    .HasForeignKey(jr => jr.CustomerID)
            //    .OnDelete(DeleteBehavior.NoAction);



            // Payment and Invoice
            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Invoice)
            //    .WithMany(i => i.Payments)
            //    .HasForeignKey(p => p.InvoiceNumber)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Location and Truck
            modelBuilder.Entity<Location>()
                .HasOne(l => l.Truck)
                .WithMany(t => t.Locations)
                .HasForeignKey(l => l.TruckID)
                .OnDelete(DeleteBehavior.Cascade);

            //// Driver and Company
            //modelBuilder.Entity<Driver>()
            //    .HasOne(d => d.Company)
            //    .WithMany(c => c.Drivers)
            //    .HasForeignKey(d => d.CompanyID)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Truck and TruckType
            //modelBuilder.Entity<Truck>()
            //    .HasOne(t => t.TruckType)
            //    .WithMany(tt => tt.Trucks)
            //    .HasForeignKey(t => t.TruckTypeID)
            //    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CompanyCustomer>()
            .HasKey(cc => cc.TXNID);

            modelBuilder.Entity<CompanyCustomer>()
           .HasKey(cc => new { cc.CustomerId, cc.CompanyId }); // Composite primary key

            modelBuilder.Entity<CompanyCustomer>()
                .HasOne(cc => cc.Customer)
                .WithMany()
                .HasForeignKey(cc => cc.CustomerId);

            modelBuilder.Entity<CompanyCustomer>()
                .HasOne(cc => cc.Company)
                .WithMany()
                .HasForeignKey(cc => cc.CompanyId);

            //Driver And TruckType ManyToMany
            modelBuilder.Entity<Driver>()
              .HasMany(d => d.TruckTypes)
              .WithMany(t => t.Drivers)
              .UsingEntity<Dictionary<string, object>>(
                  "DriverTruckType",
                  j => j
                      .HasOne<TruckType>()
                      .WithMany()
                      .HasForeignKey("TruckTypeID")
                      .HasConstraintName("FK_DriverTruckType_TruckType")
                      .OnDelete(DeleteBehavior.Cascade),
                  j => j
                      .HasOne<Driver>()
                      .WithMany()
                      .HasForeignKey("DriverID")
                      .HasConstraintName("FK_DriverTruckType_Driver")
                      .OnDelete(DeleteBehavior.Cascade)
              );
        }

        #endregion
    }
}
