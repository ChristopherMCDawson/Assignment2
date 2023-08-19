using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Controllers
{
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: Fans
        public async Task<IActionResult> Index(int? ID)
        {
            var viewModel = new SportClubViewModel
            {
                Fans = await _context.Fans
              .Include(i => i.Subscriptions)
              .AsNoTracking()
              .OrderBy(i => i.LastName)
              .ToListAsync()
            };
            if (ID != null)
            {
                ViewData["ClubID"] = ID;
                var clubIds = _context.Subscriptions.Where(f => f.FanId == ID).Select(f => f.SportClubId).ToList();
                viewModel.SportClubs = await _context.SportClubs.Where(s => clubIds.Contains(s.Id)).ToListAsync();

            }
            return View(viewModel);
        }
        public async Task<IActionResult> EditSubscriptions(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var clubs = await _context.SportClubs.ToListAsync();
            var fan = await _context.Fans.Include(i => i.Subscriptions).FirstOrDefaultAsync(f => f.ID == ID);

            var viewModel = new FanSubscriptionViewModel
            {
                Fan = fan,
                Subscriptions = clubs.Select(sport => new SportClubSubscriptionViewModel
                {
                    SportClubId = sport.Id,
                    Title = sport.Title,
                    IsMember = fan.Subscriptions?.Any(sub => sub.SportClubId == sport.Id) ?? false
                })
            };

            return View(viewModel);
        }
        public async Task<IActionResult> AddSub(int fanID, string clubID)
        {
            var fan = await _context.Fans.Include(i => i.Subscriptions).FirstOrDefaultAsync(f => f.ID == fanID);
            if (fan != null)
            {
                fan.Subscriptions.Add(new Subscription { FanId = fanID, SportClubId = clubID });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditSubscriptions), new { ID = fanID });
        }

        public async Task<IActionResult> RemoveSub(int fanID, string clubID)
        {
            var fan = await _context.Fans.Include(i => i.Subscriptions).FirstOrDefaultAsync(f => f.ID == fanID);
            if (fan != null)
            {
                var subscription = fan.Subscriptions.FirstOrDefault(s => s.SportClubId == clubID);
                if (subscription != null)
                {
                    _context.Subscriptions.Remove(subscription);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(EditSubscriptions), new { ID = fanID });
        }


        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.ID))
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
            return View(fan);
        }

        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
            return _context.Fans.Any(e => e.ID == id);
        }
    }
}