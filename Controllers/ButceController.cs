using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;

namespace BudgetApp.Controllers;

public class ButceController : Controller
{
    private readonly AppDbContext _context;

    public ButceController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? ay, int? yil)
    {
        int secilenYil = yil ?? DateTime.Today.Year;
        int secilenAy  = ay  ?? DateTime.Today.Month;

        var kategoriler = await _context.Kategoriler
            .Include(k => k.Harcamalar.Where(h => h.Tarih.Year == secilenYil && h.Tarih.Month == secilenAy))
            .ToListAsync();

        ViewBag.SecilenAy  = secilenAy;
        ViewBag.SecilenYil = secilenYil;
        return View(kategoriler);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LimitGuncelle(int id, decimal? aylikLimit)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null) return NotFound();

        kategori.AylikLimit = aylikLimit > 0 ? aylikLimit : null;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
