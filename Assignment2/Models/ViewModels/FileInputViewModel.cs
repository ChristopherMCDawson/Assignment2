﻿using Assignment2.Models;

namespace Assignment2.Models.ViewModels
{
    public class FileInputViewModel
    {
        public string SportClubId { get; set; }

        public string SportClubTitle { get; set; }
        public IFormFile File { get; set; }
    }

}