using Microsoft.EntityFrameworkCore;


namespace CheburekInfrastructure.Models;

public class CheburekMVCContext : DbContext
{


    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;
    public CheburekMVCContext(DbContextOptions<CheburekMVCContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey(c => c.ProductId);
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<User>()
    //        .HasMany(u => u.Carts)
    //        .WithOne(c => c.User)
    //        .HasForeignKey(c => c.UserId)
    //        .OnDelete(DeleteBehavior.NoAction); // Вимикає каскадне видалення для зв'язку з кошиком

    //    modelBuilder.Entity<User>()
    //        .HasMany(u => u.Orders)
    //        .WithOne(o => o.User)
    //        .HasForeignKey(o => o.UserId)
    //        .OnDelete(DeleteBehavior.NoAction); // Вимикає каскадне видалення для замовлень

    //    modelBuilder.Entity<Cart>()
    //        .HasMany(c => c.CartProducts)
    //        .WithOne(cp => cp.Cart)
    //        .HasForeignKey(cp => cp.CartId)
    //        .OnDelete(DeleteBehavior.Cascade); // Кошик можна видаляти разом з продуктами

    //    modelBuilder.Entity<Product>()
    //        .HasMany(p => p.CartProducts)
    //        .WithOne(cp => cp.Product)
    //        .HasForeignKey(cp => cp.ProductId)
    //        .OnDelete(DeleteBehavior.Restrict); // Вимкнено каскадне видалення для продуктів

    //    modelBuilder.Entity<Order>()
    //        .HasOne(o => o.Cart)
    //        .WithMany()
    //        .HasForeignKey(o => o.CartId)
    //        .OnDelete(DeleteBehavior.NoAction); // Каскадне видалення для Cart в Order вимкнено


    //}
}




