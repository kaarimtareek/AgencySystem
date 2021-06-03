using Newtonsoft.Json;

using System.Web;

namespace PressAgencyApp.ViewModels.Post
{
    public class PostViewModelU
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public int CategoryId { get; set; }
    }
}