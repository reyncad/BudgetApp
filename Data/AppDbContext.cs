using Microsoft.EntityFrameworkCore;
using BudgetApp.Models;

namespace BudgetApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Kategori> Kategoriler { get; set; }
    public DbSet<Harcama> Harcamalar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Yiyecek & İçecek" },
            new Kategori { Id = 2, Ad = "Ulaşım" },
            new Kategori { Id = 3, Ad = "Faturalar" },
            new Kategori { Id = 4, Ad = "Eğlence" },
            new Kategori { Id = 5, Ad = "Sağlık" }
        );
    }
}
