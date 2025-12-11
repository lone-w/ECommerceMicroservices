using Microsoft.EntityFrameworkCore;

namespace Product.API.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Models.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure PostgreSQL-specific settings
            modelBuilder.Entity<Models.Product>(entity =>
            {
                entity.ToTable("products"); // PostgreSQL convention: lowercase table names

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn(); // PostgreSQL identity column

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .IsRequired();

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date");

                // Create index for better query performance
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Name);
            });
        }
    }
}
