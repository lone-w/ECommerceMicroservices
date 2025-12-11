using Microsoft.EntityFrameworkCore;

namespace Order.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Orders table configuration
            modelBuilder.Entity<Models.Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .IsRequired();

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.ShippingAddress)
                    .HasColumnName("shipping_address")
                    .HasMaxLength(500);

                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.OrderDate);
            });

            // OrderItems table configuration
            modelBuilder.Entity<Models.OrderItem>(entity =>
            {
                entity.ToTable("order_items");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .IsRequired();

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                entity.Property(e => e.ProductName)
                    .HasColumnName("product_name")
                    .HasMaxLength(200);

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .IsRequired();

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unit_price")
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.Items)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.ProductId);
            });
        }
    }
}
