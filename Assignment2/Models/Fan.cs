using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Fan
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be larger than 40 characters")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be larger than 40 characters")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

        public string FullName => $"{LastName}, {FirstName}";

        public ICollection<Subscription> Subscriptions { get; set; }
    }

}
