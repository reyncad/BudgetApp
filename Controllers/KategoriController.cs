using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;

namespace BudgetApp.Controllers;

public class KategoriController : Controller
{
    private readonly AppDbContext _context;

    public KategoriController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Harcamalar)
            .ToListAsync();
        return View(kategoriler);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Kategori kategori)
    {
        if (ModelState.IsValid)
        {
            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null) return NotFound();
        return View(kategori);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Kategori kategori)
    {
        if (id != kategori.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var kategori = await _context.Kategoriler
            .Include(k => k.Harcamalar)
            .FirstOrDefaultAsync(k => k.Id == id);
        if (kategori == null) return NotFound();
        return View(kategori);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori != null)
        {
            _context.Kategoriler.Remove(kategori);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
