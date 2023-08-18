using Assignment2.Models;

namespace Assignment2.Models.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<SportClub> SportClub { get; set; }
        public IEnumerable<News> News { get; set; }
        public IEnumerable<Fan> Fans { get; set; }
    }
}
