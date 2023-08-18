using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{
    public class SportClubsController : Controller
    {
        private readonly SportsDbContext _context;

        public SportClubsController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: SportClubs
        public async Task<IActionResult> Index(String? ID)
        {
            var viewModel = new SportClubViewModel
            {
                SportClubs = await _context.SportClub
                .Include(i => i.Subscriptions)
                .AsNoTracking()
                .OrderBy(i => i.Title)
                .ToListAsync()
            };
            if (ID != null)
            {
                ViewData["ClubID"] = ID;
                var fanIds = _context.Subscriptions.Where(s => s.SportClubID == ID).Select(s => s.FanID).ToList();
                viewModel.Fans = await _context.Fans.Where(f => fanIds.Contains(f.ID)).ToListAsync();

            }
            return View(viewModel);
        }

        // GET: SportClubs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SportClub == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClub
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // GET: SportClubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportClubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportClub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportClub);
        }

        // GET: SportClubs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SportClub == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClub.FindAsync(id);
            if (sportClub == null)
            {
                return NotFound();
            }
            return View(sportClub);
        }

        // POST: SportClubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (id != sportClub.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportClub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportClubExists(sportClub.ID))
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
            return View(sportClub);
        }

        // GET: SportClubs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClub
                .Include(sc => sc.News) // Include News navigation property
                .FirstOrDefaultAsync(m => m.ID == id);

            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // POST: SportClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClub
                .Include(sc => sc.News) // Include News navigation property
                .FirstOrDefaultAsync(s => s.ID == id);

            if (sportClub == null)
            {
                return NotFound();
            }

            if (sportClub.News != null && sportClub.News.Any())
            {
                return View("Error"); // Show an error view if the sports club has news
            }

            _context.SportClub.Remove(sportClub);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportClubExists(string id)
        {
            return _context.SportClub.Any(e => e.ID == id);
        }
    }
}