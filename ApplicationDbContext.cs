using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // connect to sqlite database
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SalesItem> SalesItems { get; set; }

    // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships using Fluent API if needed
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductID);

        modelBuilder.Entity<SalesItem>()
            .HasOne(si => si.Sale)
            .WithMany(s => s.SalesItems)
            .HasForeignKey(si => si.SaleID);

        modelBuilder.Entity<SalesItem>()
            .HasOne(si => si.Product)
            .WithMany(p => p.SalesItems)
            .HasForeignKey(si => si.ProductID);

        // Additional configurations as needed
    }
}