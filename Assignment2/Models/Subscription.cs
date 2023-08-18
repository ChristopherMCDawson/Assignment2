using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Assignment2.Models
{
    public class Subscription
    {

        public int FanId { get; set; }
        public string SportClubId { get; set; }
    }
}