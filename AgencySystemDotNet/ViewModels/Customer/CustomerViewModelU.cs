using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PressAgencyApp.ViewModels.Customer
{
    public class CustomerViewModelU
    {
        [Required]
        public int Id { get; set; }

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
    }
}