using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.ViewModels.PostQuestion
{
    public class PostQuestionViewModelR
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PostId { get; set; }
        public string Question { get; set; }
        public string Answer{ get; set; }
        public bool IsAnswered{ get; set; }
        public string EditorName { get; set; }
        public string CustomerName { get; set; }
    }
}
