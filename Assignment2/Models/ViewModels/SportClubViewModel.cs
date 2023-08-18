using Assignment2.Models;


namespace Assignment2.Models.ViewModels
{
    public class SportClubViewModel
    {
        public IEnumerable<Fan> Fans { get; set; }
        public IEnumerable<SportClub> SportClubs { get; set; }
        public IEnumerable<Subscription> Subscriptions { get; set; }

    }
}
