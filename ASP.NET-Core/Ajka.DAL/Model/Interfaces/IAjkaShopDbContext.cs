using Microsoft.EntityFrameworkCore;

namespace Ajka.DAL.Model.Interfaces
{
    public interface IAjkaShopDbContext
    {
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
    }
}
