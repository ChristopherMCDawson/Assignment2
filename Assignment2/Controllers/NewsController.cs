using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Storage.Blobs;
using Azure;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{

    public class NewsController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "news";

        public NewsController(BlobServiceClient blobServiceClient, SportsDbContext context)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: News
        public async Task<IActionResult> Index(string id)
        {
            var viewModel = new NewsViewModel
            {
                News = await _context.News
                             .Include(i => i.SportsClub)
                             .AsNoTracking()
                             .Where(s => s.SportClubId == id)
                             .ToListAsync(),

                SportClub = await _context.SportClubs.FindAsync(id)
            };

            return View(viewModel);

        }

        // GET: News/Create
        public async Task <IActionResult> Create(string id)
        {
            var club = await _context.SportClubs.FirstOrDefaultAsync(s => s.Id.Equals(id));
            var viewModel = new FileInputViewModel
            {
                SportClubId = id
            };
            if (club != null)
            {
                viewModel.SportClubTitle = club.Title;
            }

            return View(viewModel);
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string id,News news, IFormFile imageFile)
        {
            if (file == null || file.Length == 0 || !ModelState.IsValid)
            {
                return View("Error");
            }

            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }

            try
            {
                string randomFileName = Path.GetRandomFileName();
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(randomFileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }
                // Save the prediction to the database
                news.FileName = randomFileName;
                news.Url = blockBlob.Uri.ToString();
                news.SportClubId = id;
            }
            catch (RequestFailedException)
            {
                return RedirectToAction(nameof(Index), new { id });
            }

            if (news != null && ModelState.IsValid)
            {
                _context.News.Add(news);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            var club = await _context.SportClubs.FirstOrDefaultAsync(n => n.Id.Equals(news.SportClubId));
            if (club == null)
            {
                return BadRequest();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'SportsDbContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = news.SportClubId });
        }

        private bool NewsExists(string id)
        {
            return _context.News.Any(e => e.SportClubId == id);
        }
    }
}
