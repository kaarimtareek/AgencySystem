using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.PostQuestion;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace AgencySystemDotNet.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext context;

        public CustomerService(AppDbContext context)
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
            string uniqueFileName = "";

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
            return context.Users.AsNoTracking().Any(x => x.Email == Email);
        }

        private bool IsPostExist(int postId)
        {
            return context.Posts.AsNoTracking().Any(x => x.Id == postId && !x.IsDeleted);
        }

        private bool IsCustomerExist(int customerId)
        {
            return context.Users.AsNoTracking().Any(x => x.Id == customerId && x.Role == CONSTANT_USER_ROLES.CUSTOMER && !x.IsDeleted);
        }

        private bool IsPostCategoryExist(int category)
        {
            return context.LookupPostCategories.AsNoTracking().Any(x => x.Id == category);
        }

        private bool IsPhonenumberExist(string phonenumber)
        {
            return context.Users.AsNoTracking().Any(x => x.Phonenumber == phonenumber);
        }

        private bool IsPostStatusExist(string status)
        {
            return context.LookupPostStatuses.AsNoTracking().Any(x => x.Id == status);
        }

        public User Login(string email, string password)
        {
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Email == email && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
            if (user == null)
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            if (!VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            return user;
        }

        public User CreateCustomer(CustomerViewModelC viewModelC)
        {
            if (IsEmailExist(viewModelC.Email))
            {
                throw new AppException(HttpStatusCode.BadRequest, "Email Already Exist");
            }
            if (IsPhonenumberExist(viewModelC.Phonenumber))
            {
                throw new AppException(HttpStatusCode.BadRequest, "Phone number Already Exist");
            }
            var imagePath = UploadImage(viewModelC.Photo);
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(viewModelC.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = viewModelC.Email,
                FirstName = viewModelC.FirstName,
                LastName = viewModelC.LastName,
                Phonenumber = viewModelC.Phonenumber,
                Photo = imagePath,
                Role = CONSTANT_USER_ROLES.CUSTOMER,
                Salt = passwordSalt,
                PasswordHash = passwordHash,
            };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User UpdateCustomer(CustomerViewModelU customerViewModelU)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == customerViewModelU.Id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");
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

        public User ChangePassword(ChangePasswordViewModel viewModel)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == viewModel.UserId && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");
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

        public int LikePost(int userId, int postId)
        {
            if (!IsPostExist(postId))
                throw new AppException(HttpStatusCode.NotFound, "Post not found");

            if (!IsCustomerExist(userId))
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");

            var postInteraction = context.PostInteractions.SingleOrDefault(x => x.CustomerId == userId && x.PostId == postId);
            var post = context.Posts.SingleOrDefault(x => x.Id == postId);

            if (postInteraction == null)
            {
                postInteraction = new PostInteraction
                {
                    PostId = postId,
                    CustomerId = userId,
                    InteractionType = CONSTANT_INTERACTION_TYPE.LIKE,
                };
                post.LikesNumber++;
                context.PostInteractions.Add(postInteraction);
                context.SaveChanges();
                return postInteraction.Id;
            }
            if (postInteraction.InteractionType == CONSTANT_INTERACTION_TYPE.DISLIKE)
            {
                post.DislikesNumber--;
                post.LikesNumber++;
                postInteraction.InteractionType = CONSTANT_INTERACTION_TYPE.LIKE;
            }
            context.SaveChanges();
            return postInteraction.PostId;
        }

        public int DisLikePost(int userId, int postId)
        {
            if (!IsPostExist(postId))
                throw new AppException(HttpStatusCode.NotFound, "Post not found");

            if (!IsCustomerExist(userId))
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");

            var postInteraction = context.PostInteractions.SingleOrDefault(x => x.CustomerId == userId && x.PostId == postId);
            var post = context.Posts.SingleOrDefault(x => x.Id == postId);

            if (postInteraction == null)
            {
                postInteraction = new PostInteraction
                {
                    PostId = postId,
                    CustomerId = userId,
                    InteractionType = CONSTANT_INTERACTION_TYPE.DISLIKE,
                };
                post.DislikesNumber++;
                context.PostInteractions.Add(postInteraction);
                context.SaveChanges();
                return postInteraction.Id;
            }
            if (postInteraction.InteractionType == CONSTANT_INTERACTION_TYPE.LIKE)
            {
                postInteraction.InteractionType = CONSTANT_INTERACTION_TYPE.DISLIKE;
                post.DislikesNumber++;
                post.LikesNumber--;
            }
            context.SaveChanges();
            return postInteraction.PostId;
        }

        public Post GetPost(int postId, int customerId = 0)
        {
            if (!IsPostExist(postId))
                throw new AppException(HttpStatusCode.NotFound, "Post not found");
            var post = context.Posts.Single(x => x.Id == postId);
            if (customerId != 0)
            {
                if (!IsCustomerExist(customerId))
                    throw new AppException(HttpStatusCode.NotFound, "Customer not found");

                var postView = context.PostViews.SingleOrDefault(x => x.PostId == postId && x.CustomerId == customerId);
                if (postView == null)
                {
                    postView = new PostView
                    {
                        PostId = postId,
                        CustomerId = customerId,
                    };
                    post.ViewersNumber++;
                    context.PostViews.Add(postView);
                    context.SaveChanges();
                }
                return post;
            }
            return post;
        }

        public List<Post> GetPosts(int? categoryId)
        {
            if (!categoryId.HasValue)
                return context.Posts.AsNoTracking().Where(x => !x.IsDeleted && x.StatusId == CONSTANT_POST_STATUS.APPROVED).ToList();
            return context.Posts.AsNoTracking().Where(x => x.CategoryId == categoryId.Value && !x.IsDeleted && x.StatusId == CONSTANT_POST_STATUS.APPROVED).ToList();
        }

        public List<Post> GetEditorPosts(int editorId)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId).ToList();
        }

        public List<Post> GetEditorPostsByStatus(int editorId, string status)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId && x.StatusId == status).ToList();
        }

        public PostQuestion CreateQuestion(PostQuestionViewModelC viewModelC)
        {
            if (!IsPostExist(viewModelC.PostId))
                throw new AppException(HttpStatusCode.NotFound, "Post not found");
            if (!IsCustomerExist(viewModelC.CustomerId))
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");
            var postQuestion = new PostQuestion
            {
                CustomerId = viewModelC.CustomerId,
                PostId = viewModelC.PostId,
                Question = viewModelC.Question,
                IsAnswered = false,
            };
            context.PostQuestions.Add(postQuestion);
            context.SaveChanges();
            return postQuestion;
        }

        public List<User> GetEditors()
        {
            return context.Users.AsNoTracking().Where(x => x.Role == CONSTANT_USER_ROLES.EDTIOR).ToList();
        }

        public User GetEditor(int id)
        {
            return context.Users.AsNoTracking().SingleOrDefault(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.EDTIOR);
        }

        public User GetCustomer(int id)
        {
            return context.Users.SingleOrDefault(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.CUSTOMER && !x.IsDeleted);
        }

        public int SavePost(int customerId, int postId)
        {
            if (!IsPostExist(postId))
                throw new AppException(HttpStatusCode.NotFound, "Post not found");
            var post = context.Posts.Single(x => x.Id == postId);

            if (!IsCustomerExist(customerId))
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");

            var postView = context.SavedPosts.SingleOrDefault(x => x.PostId == postId && x.CustomerId == customerId);
            if (postView == null)
            {
                postView = new SavedPost
                {
                    PostId = postId,
                    CustomerId = customerId,
                };
                context.SavedPosts.Add(postView);
                context.SaveChanges();
            }
            return postView.PostId;
        }
        public List<Post>GetSavedPosts(int customerId)
        {
            if (!IsCustomerExist(customerId))
                throw new AppException(HttpStatusCode.NotFound, "Customer not found");
            var postsIds = context.SavedPosts.Where(x => x.CustomerId == customerId).Select(x => x.PostId).ToList();
            var posts = context.Posts.Where(x => postsIds.Contains(x.Id)).ToList();
            return posts;

        }
        public List<Post>SearchPosts(string searchValue)
        {
            var posts = context.Posts.Where(x => !x.IsDeleted  && x.StatusId == CONSTANT_POST_STATUS.APPROVED && (x.Title.Contains(searchValue) || x.Body.Contains(searchValue) ||x.Editor.FirstName.Contains(searchValue) || x.Editor.LastName.Contains(searchValue) || x.Category.Title.Contains(searchValue))).ToList();
            return posts;

        }
    }
}