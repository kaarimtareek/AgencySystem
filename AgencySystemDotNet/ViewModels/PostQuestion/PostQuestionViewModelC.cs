using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.ViewModels.PostQuestion
{
    public class PostQuestionViewModelC
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
