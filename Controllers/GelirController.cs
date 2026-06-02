using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Data;
using BudgetApp.Models;

namespace BudgetApp.Controllers;

public class GelirController : Controller
{
    private readonly AppDbContext _context;

    public GelirController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? ay, int? yil)
    {
        int secilenYil = yil ?? DateTime.Today.Year;
        int secilenAy  = ay  ?? DateTime.Today.Month;

        var gelirler = await _context.Gelirler
            .Where(g => g.Tarih.Year == secilenYil && g.Tarih.Month == secilenAy)
            .OrderByDescending(g => g.Tarih)
            .ToListAsync();

        ViewBag.SecilenAy  = secilenAy;
        ViewBag.SecilenYil = secilenYil;
        ViewBag.Toplam     = gelirler.Sum(g => g.Miktar);
        return View(gelirler);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Gelir gelir)
    {
        if (ModelState.IsValid)
        {
            _context.Gelirler.Add(gelir);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(gelir);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var gelir = await _context.Gelirler.FindAsync(id);
        if (gelir == null) return NotFound();
        return View(gelir);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Gelir gelir)
    {
        if (id != gelir.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(gelir);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(gelir);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var gelir = await _context.Gelirler.FindAsync(id);
        if (gelir == null) return NotFound();
        return View(gelir);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gelir = await _context.Gelirler.FindAsync(id);
        if (gelir != null)
        {
            _context.Gelirler.Remove(gelir);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
