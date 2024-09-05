using Microsoft.EntityFrameworkCore;

namespace Order.API.Models.Context;

public class OrderAPIDBContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }






    public OrderAPIDBContext(DbContextOptions options): base(options)// Dependecies injection dan contexti kullanmak istiyorsak bunu eklememiz lazim bunu eklememiz lazim aspnetcore projelerinde
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