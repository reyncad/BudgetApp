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

    public async Task<IActionResult> Index()
    {
        var harcamalar = await _context.Harcamalar
            .Include(h => h.Kategori)
            .OrderByDescending(h => h.Tarih)
            .ToListAsync();

        ViewBag.ToplamHarcama = harcamalar.Sum(h => h.Miktar);
        ViewBag.BuAyHarcama = harcamalar
            .Where(h => h.Tarih.Month == DateTime.Today.Month && h.Tarih.Year == DateTime.Today.Year)
            .Sum(h => h.Miktar);

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
