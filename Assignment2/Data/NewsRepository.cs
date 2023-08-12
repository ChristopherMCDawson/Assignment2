using System.Linq;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;

namespace Assignment2.Data.Repositories
{
    public class NewsRepository
    {
        private readonly SportsDbContext _context;

        public NewsRepository(SportsDbContext context)
        {
            _context = context;
        }

        public IQueryable<News> GetAllNews()
        {
            return _context.News.AsQueryable();
        }

        public News GetNewsById(int id)
        {
            return _context.News.FirstOrDefault(news => news.Id == id);
        }

        public void AddNews(News news)
        {
            _context.News.Add(news);
        }

        public void AddNews(News news, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                news.ImagePath = UploadImage(imageFile);
            }

            _context.News.Add(news);
        }

        public void UpdateNews(News news, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                news.ImagePath = UploadImage(imageFile);
            }

            _context.News.Update(news);
        }

        private string UploadImage(IFormFile imageFile)
        {
            string uniqueFileName = null;

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        public void UpdateNews(News news)
        {
            _context.News.Update(news);
        }

        public void RemoveNews(News news)
        {
            _context.News.Remove(news);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
