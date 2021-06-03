using AgencySystemDotNet.ViewModels.Admin;

using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;

using System.Collections.Generic;

namespace PressAgencyApp.Services
{
    public interface IAdminService
    {
        User UpdateAdmin(AdminViewModelU adminViewModelU);
        User GetAdmin(int id);
        bool ChangePostStatus(int postId, string statusId);
        User CreateCustomer(CustomerViewModelC viewModelC);
        User CreateEditor(EditorViewModelC viewModelC);
        bool DeleteCustomer(int id);
        bool DeleteEditor(int id);
        bool DeletePost(int id);
        User GetCustomer(int id);
        List<User> GetCustomers();
        User GetEditor(int id);
        List<Post> GetEditorPosts(int editorId);
        List<Post> GetEditorPostsByStatus(int editorId, string status);
        List<User> GetEditors();
        Post GetPostById(int id);
        List<Post> GetPosts(string status = "");
        User Login(string email, string password);
        Post UpdatePost(PostViewModelU viewModelU);
        User ChangePassword(ChangePasswordViewModel viewModel);
    }
}