using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;

namespace BudgetApp.Controllers;

public class HedefController : Controller
{
    private readonly AppDbContext _context;

    public HedefController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var hedefler = (await _context.Hedefler.ToListAsync())
            .OrderBy(h => h.Tamamlandi)
            .ThenBy(h => h.HedefTarihi)
            .ToList();
        return View(hedefler);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Hedef hedef)
    {
        if (ModelState.IsValid)
        {
            hedef.OlusturmaTarihi = DateTime.Today;
            _context.Hedefler.Add(hedef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hedef);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var hedef = await _context.Hedefler.FindAsync(id);
        if (hedef == null) return NotFound();
        return View(hedef);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Hedef hedef)
    {
        if (id != hedef.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(hedef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hedef);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ParaEkle(int id, decimal miktar)
    {
        var hedef = await _context.Hedefler.FindAsync(id);
        if (hedef == null) return NotFound();
        hedef.BirikmisMiktar = Math.Min(hedef.HedefMiktar, hedef.BirikmisMiktar + miktar);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var hedef = await _context.Hedefler.FindAsync(id);
        if (hedef == null) return NotFound();
        return View(hedef);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hedef = await _context.Hedefler.FindAsync(id);
        if (hedef != null)
        {
            _context.Hedefler.Remove(hedef);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
