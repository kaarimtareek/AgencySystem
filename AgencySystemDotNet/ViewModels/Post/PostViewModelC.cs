using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PressAgencyApp.ViewModels.Post
{
    public class PostViewModelC
    {
        //[Required]
        public int EditorId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(50)]
        [Required]
        public string Body { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}