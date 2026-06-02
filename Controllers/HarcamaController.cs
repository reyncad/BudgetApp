using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;

namespace BudgetApp.Controllers;

public class HarcamaController : Controller
{
    private readonly AppDbContext _context;

    public HarcamaController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? ay, int? yil, int? kategoriId)
    {
        int secilenYil = yil ?? DateTime.Today.Year;
        int secilenAy  = ay  ?? DateTime.Today.Month;

        var query = _context.Harcamalar
            .Include(h => h.Kategori)
            .Where(h => h.Tarih.Year == secilenYil && h.Tarih.Month == secilenAy);

        if (kategoriId.HasValue)
            query = query.Where(h => h.KategoriId == kategoriId.Value);

        var harcamalar = await query.OrderByDescending(h => h.Tarih).ToListAsync();

        // Bütçe limiti kontrolü: o aydaki kategori bazlı harcamalar
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Harcamalar.Where(h => h.Tarih.Year == secilenYil && h.Tarih.Month == secilenAy))
            .ToListAsync();

        ViewBag.SecilenAy    = secilenAy;
        ViewBag.SecilenYil   = secilenYil;
        ViewBag.SecilenKatId = kategoriId;
        ViewBag.Kategoriler  = new SelectList(await _context.Kategoriler.ToListAsync(), "Id", "Ad", kategoriId);
        ViewBag.Toplam       = harcamalar.Sum(h => h.Miktar);
        ViewBag.LimitUyarilari = kategoriler
            .Where(k => k.AylikLimit.HasValue && k.Harcamalar.Sum(h => h.Miktar) > k.AylikLimit.Value)
            .Select(k => new { k.Ad, Harcanan = k.Harcamalar.Sum(h => h.Miktar), k.AylikLimit })
            .ToList<dynamic>();

        return View(harcamalar);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Kategoriler = new SelectList(await _context.Kategoriler.ToListAsync(), "Id", "Ad");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Harcama harcama)
    {
        if (ModelState.IsValid)
        {
            _context.Harcamalar.Add(harcama);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Kategoriler = new SelectList(await _context.Kategoriler.ToListAsync(), "Id", "Ad", harcama.KategoriId);
        return View(harcama);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var harcama = await _context.Harcamalar.FindAsync(id);
        if (harcama == null) return NotFound();
        ViewBag.Kategoriler = new SelectList(await _context.Kategoriler.ToListAsync(), "Id", "Ad", harcama.KategoriId);
        return View(harcama);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Harcama harcama)
    {
        if (id != harcama.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(harcama);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Kategoriler = new SelectList(await _context.Kategoriler.ToListAsync(), "Id", "Ad", harcama.KategoriId);
        return View(harcama);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var harcama = await _context.Harcamalar
            .Include(h => h.Kategori)
            .FirstOrDefaultAsync(h => h.Id == id);
        if (harcama == null) return NotFound();
        return View(harcama);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var harcama = await _context.Harcamalar.FindAsync(id);
        if (harcama != null)
        {
            _context.Harcamalar.Remove(harcama);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
