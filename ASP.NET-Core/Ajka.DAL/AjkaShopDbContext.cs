using Ajka.DAL.Model;
using Ajka.DAL.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ajka.DAL
{
    public class AjkaShopDbContext : DbContext, IAjkaShopDbContext
    {
        public AjkaShopDbContext(DbContextOptions<AjkaShopDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ItemCard> ItemCard { get; set; }
        public DbSet<ItemCardImage> ItemCardImage { get; set; }
        public DbSet<ItemCardSizePrice> ItemCardSizePrice { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        public DbSet<IndividualVariable> IndividualVariable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(a => a.IsValid);

            modelBuilder.Entity<Category>().HasIndex(a => a.IsValid);

            modelBuilder.Entity<ItemCard>().HasIndex(a => a.State);
            modelBuilder.Entity<ItemCard>().Property(r => r.Price)
                        .HasColumnType("decimal(12,2)");
            modelBuilder.Entity<ItemCard>().HasIndex(a => a.IsAdlerProduct);

            modelBuilder.Entity<ItemCardSizePrice>().Property(r => r.Price)
                        .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Order>().Property(r => r.Discount)
                        .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<InvoiceItem>().Property(r => r.PricePerPiece)
                        .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Invoice>().HasIndex(a => a.ReleaseDate);
        }
    }
}
