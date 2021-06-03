using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.ViewModels
{
    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel()
        {

        }
        public ChangePasswordViewModel(int id )
        {
            UserId = id;
            NewPassword = "";
            OldPassword = "";
        }
       
        public int UserId { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
        public string Role { get; set; }
    }
}
