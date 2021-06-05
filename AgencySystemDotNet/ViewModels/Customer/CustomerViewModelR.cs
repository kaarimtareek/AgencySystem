using AgencySystemDotNet.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.ViewModels.Customer
{
    public class CustomerViewModelR : BaseUserViewModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string Phonenumber { get; set; }
        public string Photo { get; set; }
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
