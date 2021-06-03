using AgencySystemDotNet.ViewModels.Editor;

using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Post;

using System.Collections.Generic;

namespace PressAgencyApp.Services
{
    public interface IEditorService
    {
        User GetEditor(int id);
        User UpdateEditor(EditorViewModelU customerViewModelU);
        PostQuestion AnswerQuestion(int questionId, string answer);
        Post CreatePost(PostViewModelC viewModelC);
        bool DeletePost(int id);
        List<Post> GetEditorPosts(int editorId);
        List<Post> GetEditorPostsByStatus(int editorId, string status);
        Post GetPostById(int id);
        PostQuestion GetPostQuestion(int questionId);
        List<PostQuestion> GetPostQuestions(int postId);
        List<PostQuestion> GetPostUnAnsweredQuestions(int postId);
        List<PostQuestion> GetUnAnsweredQuestions(int editorId);
        User Login(string email, string password);
        Post UpdatePost(PostViewModelU viewModelU);
        User ChangePassword(ChangePasswordViewModel viewModel);
    }
}