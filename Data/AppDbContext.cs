using Microsoft.EntityFrameworkCore;
using BudgetApp.Models;

namespace BudgetApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Kategori> Kategoriler { get; set; }
    public DbSet<Harcama> Harcamalar { get; set; }
    public DbSet<Gelir> Gelirler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Yiyecek & İçecek", AylikLimit = 3000 },
            new Kategori { Id = 2, Ad = "Ulaşım", AylikLimit = 1500 },
            new Kategori { Id = 3, Ad = "Faturalar", AylikLimit = 2000 },
            new Kategori { Id = 4, Ad = "Eğlence", AylikLimit = 1000 },
            new Kategori { Id = 5, Ad = "Sağlık", AylikLimit = null }
        );
    }
}
