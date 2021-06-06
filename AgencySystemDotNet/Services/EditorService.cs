using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Models;
using PressAgencyApp.ViewModels.Post;
using System.Web.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using AgencySystemDotNet.ViewModels.Editor;
using PressAgencyApp.ViewModels;

namespace AgencySystemDotNet.Services
{
    public class EditorService : IEditorService
    {
        private readonly AppDbContext context;

        public EditorService(AppDbContext context)
        {
            this.context = context;
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string UploadImage(HttpPostedFileBase image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                string uploadsFolder = Path.Combine("~/images");
                string uploadRoot = HostingEnvironment.MapPath("/images"); 
                string folder = string.Format(uploadRoot);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    //ViewBag.Message = "Folder " + folderName.ToString() + " created successfully!";
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(HostingEnvironment.MapPath("/images/"), uniqueFileName);
                //Path.GetFullPath(,filePath)));

                image.SaveAs(filePath);
            }
            return uniqueFileName;
        }

        private bool IsEmailExist(string Email)
        {
            return context.Users.Any(x => x.Email == Email);
        }

        private bool IsPostCategoryExist(int category)
        {
            return context.LookupPostCategories.Any(x => x.Id == category);
        }

        private bool IsPhonenumberExist(string phonenumber)
        {
            return context.Users.Any(x => x.Phonenumber == phonenumber);
        }

        private bool IsPostStatusExist(string status)
        {
            return context.LookupPostStatuses.Any(x => x.Id == status);
        }
        public User UpdateEditor(EditorViewModelU customerViewModelU)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == customerViewModelU.Id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.EDTIOR);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Editor not found");
            }
            user.FirstName = customerViewModelU.FirstName;
            user.LastName = customerViewModelU.LastName;
            if (user.Email != customerViewModelU.Email)
            {
                if (IsEmailExist(customerViewModelU.Email))
                    throw new AppException(HttpStatusCode.BadRequest, "Email Already Exist");
                else
                    user.Email = customerViewModelU.Email;
            }
            if (user.Phonenumber != customerViewModelU.Phonenumber)
            {
                if (IsPhonenumberExist(customerViewModelU.Phonenumber))
                    throw new AppException(HttpStatusCode.BadRequest, "Phone number Already Exist");
                else
                    user.Phonenumber = customerViewModelU.Phonenumber;
            }
            if (customerViewModelU.Photo != null)
                user.Photo = UploadImage(customerViewModelU.Photo);
            context.SaveChanges();
            return user;
        }
        public User GetEditor(int id)
        {
            var user = context.Users.AsNoTracking().SingleOrDefault(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.EDTIOR && !x.IsDeleted);
            if (user == null)
                throw new AppException(HttpStatusCode.NotFound, "Editor not found");
            return user;
        }
        public User Login(string email, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email && x.Role == CONSTANT_USER_ROLES.EDTIOR);
            if (user == null)
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            if (!VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            return user;
        }

        public Post CreatePost(PostViewModelC viewModelC)
        {
            if (!IsPostCategoryExist(viewModelC.CategoryId))
            {
                throw new AppException(HttpStatusCode.BadRequest, "Category Not Found");
            }
            var post = new Post
            {
                CategoryId = viewModelC.CategoryId,
                Body = viewModelC.Body,
                Title = viewModelC.Title,
                EditorId = viewModelC.EditorId,
                StatusId = CONSTANT_POST_STATUS.PENDING,
                Photo = viewModelC.Photo == null ? "" : UploadImage(viewModelC.Photo),
            };
            context.Posts.Add(post);
            context.SaveChanges();
            return post;
        }

        public Post UpdatePost(PostViewModelU viewModelU)
        {
            var post = context.Posts.SingleOrDefault(x => x.Id == viewModelU.Id && !x.IsDeleted);
            if (post == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Post Not Found");
            }
            post.Body = viewModelU.Body;
            post.CategoryId = viewModelU.CategoryId;
            post.Title = viewModelU.Title;
            post.StatusId = CONSTANT_POST_STATUS.PENDING;
            if (viewModelU.Photo != null)
                post.Photo = UploadImage(viewModelU.Photo);
            context.SaveChanges();
            return post;
        }

        public bool DeletePost(int id)
        {
            var post = context.Posts.SingleOrDefault(x => x.Id == id && !x.IsDeleted);
            if (post == null)
                throw new AppException(HttpStatusCode.NotFound, "Post Not Found");
            post.IsDeleted = true;
            context.SaveChanges();
            return true;
        }

        public List<Post> GetEditorPosts(int editorId)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId && !x.IsDeleted).ToList();
        }

        public List<Post> GetEditorPostsByStatus(int editorId, string status)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId && x.StatusId == status && !x.IsDeleted).ToList();
        }

        public Post GetPostById(int id)
        {
            //then include customer
            return context.Posts.AsNoTracking().SingleOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public List<PostQuestion> GetPostQuestions(int postId)
        {
            return context.PostQuestions.AsNoTracking().Where(x => x.PostId == postId && !x.IsDeleted).ToList();
        }

        public PostQuestion GetPostQuestion(int questionId)
        {
            return context.PostQuestions.AsNoTracking().SingleOrDefault(x => x.Id == questionId && !x.IsDeleted);
        }

        public List<PostQuestion> GetPostUnAnsweredQuestions(int postId)
        {
            return context.PostQuestions.AsNoTracking().Where(x => x.PostId == postId && !x.IsAnswered && !x.IsDeleted).ToList();
        }

        public List<PostQuestion> GetUnAnsweredQuestions(int editorId)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId && !x.IsDeleted).SelectMany(x => x.Questions).ToList();
        }
        
        public PostQuestion AnswerQuestion(int questionId, string answer)
        {
            var question = context.PostQuestions.SingleOrDefault(x => x.Id == questionId);
            if (question == null)
                throw new AppException(HttpStatusCode.NotFound, "Question Not Found");
            if (question.IsAnswered)
                throw new AppException(HttpStatusCode.BadRequest, "Question Already answered");
            question.Answer = answer;
            question.IsAnswered = true;
            context.SaveChanges();
            return question;
        }
        public User ChangePassword(ChangePasswordViewModel viewModel)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == viewModel.UserId && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.EDTIOR);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Editor not found");
            }
            if (!VerifyPasswordHash(viewModel.OldPassword, user.PasswordHash, user.Salt))
                throw new AppException(HttpStatusCode.BadRequest, "Old Password doesnt match");
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(viewModel.NewPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.Salt = passwordSalt;
            context.SaveChanges();
            return user;
        }
    }
}