using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.PostQuestion;

using System.Collections.Generic;

namespace AgencySystemDotNet.Services
{
    public interface ICustomerService
    {
        User ChangePassword(ChangePasswordViewModel viewModel);
        User CreateCustomer(CustomerViewModelC viewModelC);
        User GetCustomer(int id);
        PostQuestion CreateQuestion(PostQuestionViewModelC viewModelC);
        int DisLikePost(int userId, int postId);
        User GetEditor(int id);
        List<Post> GetEditorPosts(int editorId);
        List<Post> GetEditorPostsByStatus(int editorId, string status);
        List<User> GetEditors();
        Post GetPost(int postId, int customerId = 0);
        List<Post> GetPosts(int? categoryId);
        int LikePost(int userId, int postId);
        User Login(string email, string password);
        User UpdateCustomer(CustomerViewModelU customerViewModelU);
        int SavePost(int customerId, int postId);
        List<Post> GetSavedPosts(int customerId);
        List<Post> GetSavedPosts(string searchValue);
    }

}