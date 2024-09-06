using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;

namespace Stock.API.Models.Contexts;

public class StockAPIDBContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public StockAPIDBContext(DbContextOptions options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}