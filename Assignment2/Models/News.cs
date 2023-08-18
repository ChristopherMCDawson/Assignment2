using System.ComponentModel.DataAnnotations;


namespace Assignment2.Models
{
    public class News
    {
        public int NewsId { get; set; }


        [StringLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set;}

        [Url]
        public string Url { get; set;}
        public string SportClubId { get; set;}
        public SportClub SportClub { get; set;}
    }
}
