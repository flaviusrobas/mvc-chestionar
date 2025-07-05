using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppChestionar.Data;
using WebAppChestionar.Models;

namespace WebAppChestionar.Controllers
{
    public class SondajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SondajController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //Sondaj/Index
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Sondaj != null ?
                        View(await _context.Sondaj.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Sondaj'  is null.");
        }

        //Sondaj/CautInFormular
        [Authorize]
        public IActionResult CautInFormular()
        {

            return View("CautInFormular");
        }

        [Authorize]
        public async Task<IActionResult> Cautare(String CautaText)
        {
            
            return View("Index", await _context.Sondaj.Where(j => j.Intrebari.Contains(CautaText)).ToListAsync());
        }


        //Sondaj/Detalii
        public async Task<IActionResult> Detalii(int? id)
        {
            if (id == null || _context.Sondaj == null)
            {
                return NotFound();
            }

            var sondaj = await _context.Sondaj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sondaj == null)
            {
                return NotFound();
            }

            return View(sondaj);
        }

        //Sondaj/Adaugare
        [Authorize]
        public IActionResult Adaugare()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adaugare([Bind("Id,Intrebari,Raspunsuri")] Sondaj sondaj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sondaj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sondaj);
        }


        //Sondaj/Editare
        [Authorize]
        public async Task<IActionResult> Editare(int? id)
        {
            if (id == null || _context.Sondaj == null)
            {
                return NotFound();
            }

            var sondaj = await _context.Sondaj.FindAsync(id);
            if (sondaj == null)
            {
                return NotFound();
            }
            return View(sondaj);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editare(int id, [Bind("Id,Intrebari,Raspunsuri")] Sondaj sondaj)
        {
            if (id != sondaj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sondaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SondajExists(sondaj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sondaj);
        }

        //Sondaj/Stergere
        [Authorize]
        public async Task<IActionResult> Stergere(int? id)
        {
            if (id == null || _context.Sondaj == null)
            {
                return NotFound();
            }

            var sondaj = await _context.Sondaj
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sondaj == null)
            {
                return NotFound();
            }

            return View(sondaj);
        }

        [Authorize]
        [HttpPost, ActionName("Stergere")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sondaj == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sondaj'  is null.");
            }
            var sondaj = await _context.Sondaj.FindAsync(id);
            if (sondaj != null)
            {
                _context.Sondaj.Remove(sondaj);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SondajExists(int id)
        {
            return (_context.Sondaj?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
