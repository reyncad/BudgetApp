using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;
using System.Diagnostics;

namespace BudgetApp.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? ay, int? yil)
    {
        int secilenYil = yil ?? DateTime.Today.Year;
        int secilenAy  = ay  ?? DateTime.Today.Month;

        var harcamalar = await _context.Harcamalar
            .Include(h => h.Kategori)
            .Where(h => h.Tarih.Year == secilenYil && h.Tarih.Month == secilenAy)
            .ToListAsync();

        var gelirler = await _context.Gelirler
            .Where(g => g.Tarih.Year == secilenYil && g.Tarih.Month == secilenAy)
            .ToListAsync();

        var toplamGelir   = gelirler.Sum(g => g.Miktar);
        var toplamHarcama = harcamalar.Sum(h => h.Miktar);
        var netBakiye     = toplamGelir - toplamHarcama;

        // Son 6 aylık gelir/gider trend
        var aylikOzet = new List<object>();
        for (int i = 5; i >= 0; i--)
        {
            var t = new DateTime(secilenYil, secilenAy, 1).AddMonths(-i);
            var ayGelir   = (await _context.Gelirler.Where(g => g.Tarih.Year == t.Year && g.Tarih.Month == t.Month).ToListAsync()).Sum(g => g.Miktar);
            var ayHarcama = (await _context.Harcamalar.Where(h => h.Tarih.Year == t.Year && h.Tarih.Month == t.Month).ToListAsync()).Sum(h => h.Miktar);
            aylikOzet.Add(new { Etiket = t.ToString("MMM yy"), Gelir = ayGelir, Harcama = ayHarcama });
        }

        // Kategori bazlı harcama dağılımı
        var kategoriHarcama = harcamalar
            .GroupBy(h => h.Kategori?.Ad ?? "Diğer")
            .Select(g => new { Ad = g.Key, Toplam = g.Sum(h => h.Miktar) })
            .OrderByDescending(x => x.Toplam)
            .ToList();

        // Bütçe limit durumu
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Harcamalar.Where(h => h.Tarih.Year == secilenYil && h.Tarih.Month == secilenAy))
            .ToListAsync();

        var limitDurumu = kategoriler
            .Where(k => k.AylikLimit.HasValue)
            .Select(k => new
            {
                k.Ad,
                Limit    = k.AylikLimit!.Value,
                Harcanan = k.Harcamalar.Sum(h => h.Miktar),
                Oran     = k.AylikLimit.Value > 0
                    ? (int)Math.Min(100, k.Harcamalar.Sum(h => h.Miktar) / k.AylikLimit.Value * 100)
                    : 0
            })
            .OrderByDescending(x => x.Oran)
            .ToList<dynamic>();

        ViewBag.SecilenAy       = secilenAy;
        ViewBag.SecilenYil      = secilenYil;
        ViewBag.ToplamGelir     = toplamGelir;
        ViewBag.ToplamHarcama   = toplamHarcama;
        ViewBag.NetBakiye       = netBakiye;
        ViewBag.AylikOzet       = aylikOzet;
        ViewBag.KategoriHarcama = kategoriHarcama;
        ViewBag.LimitDurumu     = limitDurumu;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
