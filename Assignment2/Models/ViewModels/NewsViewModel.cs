﻿namespace Assignment2.Models.ViewModels
{
    public class NewsViewModel
    {
        public SportClub SportClub { get; set; }
        public IEnumerable<News> News { get; set; }
    }

}