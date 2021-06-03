using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PressAgencyApp.ViewModels.Customer
{
    public class CustomerViewModelC
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(50)]
        [Required]
        public string Phonenumber { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}