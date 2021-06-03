using PressAgencyApp.ViewModels.PostQuestion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.ViewModels.Post
{
    public class PostViewModelR
    {
        public int Id { get; set; }
      
        public int EditorId { get; set; }
       
        public string StatusId { get; set; }
       
        public string Title { get; set; }
        public string Body { get; set; }
        public string Photo { get; set; }
        public int ViewersNumber { get; set; }
        public int DislikesNumber { get; set; }
        public int LikesNumber { get; set; }
        public int CategoryId { get; set; }
        
        public string EditorName { get; set; }
        
        
       public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PostQuestionViewModelR> Questions { get; set; }

    }
}
