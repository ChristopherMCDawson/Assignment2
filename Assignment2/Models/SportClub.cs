using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Assignment2.Models
{
    public class SportClub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Registration Number")]
        [Required(ErrorMessage = "Registration number is required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title length should be between 3 and 50 characters.")]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Fee { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<News> News { get; set; }
    }

}
