using Microsoft.EntityFrameworkCore;

namespace Warehouse.API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        protected DatabaseContext() : base()
        {
        }

        #region Properties
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<StoredItem> StoredItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<StockTaking.StockTaking> StockTakings { get; set; }
        public DbSet<StockTaking.StockTakingItem> GetStockTakingItems { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Address
            modelBuilder.Entity<Address>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Address>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            #endregion
            #region Movement
            modelBuilder.Entity<Movement>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Movement>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.StoredItem)
                .WithMany(s => s.Movements)
                .HasForeignKey(m => m.StoredItemId);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.Position)
                .WithMany(p => p.Movements)
                .HasForeignKey(m => m.PositionId);

            modelBuilder.Entity<Movement>()
                .Property(m => m.MovementDate)
                .ValueGeneratedOnAdd();
            #endregion
            #region Position
            modelBuilder.Entity<Position>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Position>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Section)
                .WithMany(s => s.Positions)
                .HasForeignKey(p => p.SectionId);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.StoredItem)
                .WithMany(s => s.Positions)
                .HasForeignKey(p => p.StoredItemId)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region Section
            modelBuilder.Entity<Section>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Section>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Section>()
                .HasOne(s => s.Warehouse)
                .WithMany(w => w.Sections)
                .HasForeignKey(s => s.WarehouseId);
            #endregion
            #region StoredItem
            modelBuilder.Entity<StoredItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<StoredItem>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            #endregion
            #region Warehouse
            modelBuilder.Entity<Warehouse>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Warehouse>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Address)
                .WithOne(a => a.Warehouse)
                .HasForeignKey<Warehouse>(w => w.AddressId);
            #endregion

            #region StockTaking
            modelBuilder.Entity<StockTaking.StockTaking>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<StockTaking.StockTaking>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<StockTaking.StockTaking>()
                .Property(x => x.DateTime)
                .ValueGeneratedOnAdd();
            #endregion
            #region StockTakingItem
            modelBuilder.Entity<StockTaking.StockTakingItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<StockTaking.StockTakingItem>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<StockTaking.StockTakingItem>()
                .HasOne(i => i.StockTaking)
                .WithMany(s => s.StockTakingItems)
                .HasForeignKey(i => i.StockTakingId);

            modelBuilder.Entity<StockTaking.StockTakingItem>()
                .HasOne(i => i.StoredItem)
                .WithMany(s => s.StockTakingItems)
                .HasForeignKey(i => i.StoredItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTaking.StockTakingItem>()
                .HasOne(i => i.Position)
                .WithMany(p => p.StockTakingItems)
                .HasForeignKey(i => i.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
