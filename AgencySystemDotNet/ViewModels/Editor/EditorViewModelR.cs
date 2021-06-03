using PressAgencyApp.ViewModels.Post;

using System.Collections.Generic;

namespace PressAgencyApp.ViewModels.Editor
{
    public class EditorViewModelR
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phonenumber { get; set; }
       

        public string Photo { get; set; }

        public string Role { get; set; }
        public List<PostViewModelR> Posts { get; set; }
    }
}