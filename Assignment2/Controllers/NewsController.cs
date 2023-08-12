using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{

    public class NewsController : Controller
    {
        private readonly NewsRepository _newsRepository;

        public NewsController(NewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        // GET: News
        public IActionResult Index()
        {
            var news = _newsRepository.GetAllNews();
            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(News news, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                _newsRepository.AddNews(news, imageFile);
                _newsRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Edit/5
        public IActionResult Edit(int id)
        {
            var newsArticle = _newsRepository.GetNewsById(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, News news, IFormFile imageFile)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _newsRepository.UpdateNews(news, imageFile);
                _newsRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        // GET: News/Details/5
        public IActionResult Details(int id)
        {
            var newsArticle = _newsRepository.GetNewsById(id);

            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }
    }
}
