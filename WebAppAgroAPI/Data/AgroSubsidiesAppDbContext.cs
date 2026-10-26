using Microsoft.EntityFrameworkCore;

namespace WebAppAgroAPI.Data
{
    public class AgroSubsidiesAppDbContext : DbContext
    {
        public AgroSubsidiesAppDbContext()
        {
        }

        public AgroSubsidiesAppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Farmer> Farmers { get; set; } = null!;

        public DbSet<Parcel> Parcels { get; set; } = null!;

        public DbSet<RegionalUnit> RegionalUnits { get; set; } = null!;

        public DbSet<SubsidyApplication> SubsidyApplications { get; set; } = null!;

        public DbSet<ApplicationParcel> ApplicationParcels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(static entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.UserRole).HasConversion<string>();
                entity.Property(e => e.UserStatus)
                .HasConversion<string>();
                entity.Property(e => e.IsActive)
                .HasDefaultValue(true);
                entity.Property(e => e.InsertedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
            });

            modelBuilder.Entity<Employee>(static entity =>
            {
                entity.ToTable("Employees");
                entity.Property(e => e.InsertedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.UserId, "IX_Employees_UserId").IsUnique();

                entity.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Farmer>(entity =>
            {
                entity.ToTable("Farmers");
                entity.Property(e => e.InsertedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.UserId, "IX_Farmers_UserId").IsUnique();
                entity.HasIndex(e => e.TaxIdentificationNumber, "IX_Farmers_TaxIdentificationNumber").IsUnique();
                entity.HasIndex(e => e.PhoneNumber, "IX_Farmers_PhoneNumber").IsUnique();
                entity.HasOne(f => f.User)
                .WithOne(u => u.Farmer)
                .HasForeignKey<Farmer>(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Parcel>(static entity =>
            {
                entity.ToTable("Parcels");

                entity.Property(e => e.ParcelCode)
                .HasMaxLength(50)
                .IsRequired();

                entity.Property(e => e.Area)
                .HasColumnType("decimal(8,1)");

                entity.Property(e => e.CropType)
                 .HasConversion<string>();


                entity.Property(e => e.OwnershipType)
                .HasConversion<string>();

                entity.HasIndex(e => e.ParcelCode, "IX_Parcels_ParcelCode").IsUnique();

                entity.HasIndex(e => e.RegionalUnitId, "IX_Parcels_RegionalUnitId");

                entity.HasIndex(e => e.FarmerId, "IX_Parcels_FarmerId");

                entity.HasOne(p => p.Farmer) //one parcel with one farmer
                .WithMany(f => f.Parcels)    // who have many parcels
                .HasForeignKey(p => p.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.RegionalUnit)
                   .WithMany(r => r.Parcels)
                   .HasForeignKey(p => p.RegionalUnitId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SubsidyApplication>(static entity =>
            {
                entity.ToTable("SubsidyApplications");

                entity.Property(e => e.SubsidyType)
                    .HasConversion<string>();

                entity.Property(e => e.ApplicationStatus)
                    .HasConversion<string>();

                entity.Property(e => e.RejectionReason)
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ApplicationParcel>(static entity =>
            {
                entity.ToTable("ApplicationParcels");

                entity.HasKey(e => new { e.SubsidyApplicationId, e.ParcelId });

                entity.HasOne(ap => ap.SubsidyApplication)
                .WithMany(sa => sa.ApplicationParcels)
                .HasForeignKey(ap => ap.SubsidyApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ap => ap.Parcel)
                    .WithMany(p => p.ApplicationParcels)
                    .HasForeignKey(ap => ap.ParcelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RegionalUnit>(static entity =>
            {
                entity.ToTable("RegionalUnits");

                entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

                entity.HasIndex(e => e.Name, "IX_RegionalUnits_Name").IsUnique();

                entity.HasData(
                new RegionalUnit { Id = 1, Name = "ΑΙΤΩΛΟΑΚΑΡΝΑΝΙΑΣ" },
                new RegionalUnit { Id = 2, Name = "ΑΝΑΤΟΛΙΚΗΣ ΑΤΤΙΚΗΣ" },
                new RegionalUnit { Id = 3, Name = "ΑΝΔΡΟΥ" },
                new RegionalUnit { Id = 4, Name = "ΑΡΓΟΛΙΔΑΣ" },
                new RegionalUnit { Id = 5, Name = "ΑΡΚΑΔΙΑΣ" },
                new RegionalUnit { Id = 6, Name = "ΑΡΤΑΣ" },
                new RegionalUnit { Id = 7, Name = "ΑΧΑΪΑΣ" },
                new RegionalUnit { Id = 8, Name = "ΒΟΙΩΤΙΑΣ" },
                new RegionalUnit { Id = 9, Name = "ΒΟΡΕΙΟΥ ΤΟΜΕΑ ΑΘΗΝΩΝ" },
                new RegionalUnit { Id = 10, Name = "ΓΡΕΒΕΝΩΝ" },
                new RegionalUnit { Id = 11, Name = "ΔΡΑΜΑΣ" },
                new RegionalUnit { Id = 12, Name = "ΔΥΤΙΚΟΥ ΤΟΜΕΑ ΑΘΗΝΩΝ" },
                new RegionalUnit { Id = 13, Name = "ΔΥΤΙΚΗΣ ΑΤΤΙΚΗΣ" },
                new RegionalUnit { Id = 14, Name = "ΕΒΡΟΥ" },
                new RegionalUnit { Id = 15, Name = "ΕΥΒΟΙΑΣ" },
                new RegionalUnit { Id = 16, Name = "ΕΥΡΥΤΑΝΙΑΣ" },
                new RegionalUnit { Id = 17, Name = "ΖΑΚΥΝΘΟΥ" },
                new RegionalUnit { Id = 18, Name = "ΗΛΕΙΑΣ" },
                new RegionalUnit { Id = 19, Name = "ΗΜΑΘΙΑΣ" },
                new RegionalUnit { Id = 20, Name = "ΗΡΑΚΛΕΙΟΥ" },
                new RegionalUnit { Id = 21, Name = "ΘΑΣΟΥ" },
                new RegionalUnit { Id = 22, Name = "ΘΕΣΠΡΩΤΙΑΣ" },
                new RegionalUnit { Id = 23, Name = "ΘΕΣΣΑΛΟΝΙΚΗΣ" },
                new RegionalUnit { Id = 24, Name = "ΘΗΡΑΣ" },
                new RegionalUnit { Id = 25, Name = "ΙΘΑΚΗΣ" },
                new RegionalUnit { Id = 26, Name = "ΙΚΑΡΙΑΣ" },
                new RegionalUnit { Id = 27, Name = "ΙΩΑΝΝΙΝΩΝ" },
                new RegionalUnit { Id = 28, Name = "ΚΑΒΑΛΑΣ" },
                new RegionalUnit { Id = 29, Name = "ΚΑΛΥΜΝΟΥ" },
                new RegionalUnit { Id = 30, Name = "ΚΑΡΔΙΤΣΑΣ" },
                new RegionalUnit { Id = 31, Name = "ΚΑΡΠΑΘΟΥ" },
                new RegionalUnit { Id = 32, Name = "ΚΑΣΤΟΡΙΑΣ" },
                new RegionalUnit { Id = 33, Name = "ΚΕΑΣ - ΚΥΘΝΟΥ" },
                new RegionalUnit { Id = 34, Name = "ΚΕΝΤΡΙΚΟΥ ΤΟΜΕΑ ΑΘΗΝΩΝ" },
                new RegionalUnit { Id = 35, Name = "ΚΕΡΚΥΡΑΣ" },
                new RegionalUnit { Id = 36, Name = "ΚΕΦΑΛΛΗΝΙΑΣ" },
                new RegionalUnit { Id = 37, Name = "ΚΙΛΚΙΣ" },
                new RegionalUnit { Id = 38, Name = "ΚΟΖΑΝΗΣ" },
                new RegionalUnit { Id = 39, Name = "ΚΟΡΙΝΘΙΑΣ" },
                new RegionalUnit { Id = 40, Name = "ΚΩ" },
                new RegionalUnit { Id = 41, Name = "ΛΑΚΩΝΙΑΣ" },
                new RegionalUnit { Id = 42, Name = "ΛΑΡΙΣΑΣ" },
                new RegionalUnit { Id = 43, Name = "ΛΑΣΙΘΙΟΥ" },
                new RegionalUnit { Id = 44, Name = "ΛΕΣΒΟΥ" },
                new RegionalUnit { Id = 45, Name = "ΛΕΥΚΑΔΑΣ" },
                new RegionalUnit { Id = 46, Name = "ΛΗΜΝΟΥ" },
                new RegionalUnit { Id = 47, Name = "ΜΑΓΝΗΣΙΑΣ" },
                new RegionalUnit { Id = 48, Name = "ΜΕΣΣΗΝΙΑΣ" },
                new RegionalUnit { Id = 49, Name = "ΜΗΛΟΥ" },
                new RegionalUnit { Id = 50, Name = "ΜΥΚΟΝΟΥ" },
                new RegionalUnit { Id = 51, Name = "ΝΑΞΟΥ" },
                new RegionalUnit { Id = 52, Name = "ΝΗΣΩΝ" },
                new RegionalUnit { Id = 53, Name = "ΝΟΤΙΟΥ ΤΟΜΕΑ ΑΘΗΝΩΝ" },
                new RegionalUnit { Id = 54, Name = "ΞΑΝΘΗΣ" },
                new RegionalUnit { Id = 55, Name = "ΠΑΡΟΥ" },
                new RegionalUnit { Id = 56, Name = "ΠΕΙΡΑΙΩΣ" },
                new RegionalUnit { Id = 57, Name = "ΠΕΛΛΑΣ" },
                new RegionalUnit { Id = 58, Name = "ΠΙΕΡΙΑΣ" },
                new RegionalUnit { Id = 59, Name = "ΠΡΕΒΕΖΑΣ" },
                new RegionalUnit { Id = 60, Name = "ΡΕΘΥΜΝΗΣ" },
                new RegionalUnit { Id = 61, Name = "ΡΟΔΟΠΗΣ" },
                new RegionalUnit { Id = 62, Name = "ΡΟΔΟΥ" },
                new RegionalUnit { Id = 63, Name = "ΣΑΜΟΥ" },
                new RegionalUnit { Id = 64, Name = "ΣΕΡΡΩΝ" },
                new RegionalUnit { Id = 65, Name = "ΣΠΟΡΑΔΩΝ" },
                new RegionalUnit { Id = 66, Name = "ΣΥΡΟΥ" },
                new RegionalUnit { Id = 67, Name = "ΤΗΝΟΥ" },
                new RegionalUnit { Id = 68, Name = "ΤΡΙΚΑΛΩΝ" },
                new RegionalUnit { Id = 69, Name = "ΦΘΙΩΤΙΔΑΣ" },
                new RegionalUnit { Id = 70, Name = "ΦΛΩΡΙΝΑΣ" },
                new RegionalUnit { Id = 71, Name = "ΦΩΚΙΔΑΣ" },
                new RegionalUnit { Id = 72, Name = "ΧΑΛΚΙΔΙΚΗΣ" },
                new RegionalUnit { Id = 73, Name = "ΧΑΝΙΩΝ" },
                new RegionalUnit { Id = 74, Name = "ΧΙΟΥ" });
            });

        }

    }
}
