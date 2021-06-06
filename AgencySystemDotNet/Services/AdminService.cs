using AgencySystemDotNet.ViewModels.Admin;

using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace AgencySystemDotNet.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext context;
        public AdminService()
        {

        }
        public AdminService(AppDbContext context)
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

        private bool IsPhonenumberExist(string phonenumber)
        {
            return context.Users.Any(x => x.Phonenumber == phonenumber);
        }

        private bool IsPostStatusExist(string status)
        {
            return context.LookupPostStatuses.Any(x => x.Id == status);
        }

        public User Login(string email, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email && x.Role == CONSTANT_USER_ROLES.ADMIN);
            if (user == null)
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            if (!VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            return user;
        }
        public User GetAdmin(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.ADMIN);
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

        public List<User> GetCustomers()
        {
            return context.Users.AsNoTracking().Where(x => x.Role == CONSTANT_USER_ROLES.CUSTOMER).ToList();
        }

        public User GetCustomer(int id)
        {
            return context.Users.AsNoTracking().SingleOrDefault(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
        }

        public bool DeleteCustomer(int id)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
            if (user == null)
                throw new AppException(HttpStatusCode.NotFound, "user Not Found");
            user.IsDeleted = true;
            context.SaveChanges();
            return true;
        }

        public User CreateEditor(EditorViewModelC viewModelC)
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
                Role = CONSTANT_USER_ROLES.EDTIOR,
                Salt = passwordSalt,
                PasswordHash = passwordHash,
            };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public List<User> GetEditors()
        {
            return context.Users.AsNoTracking().Where(x => x.Role == CONSTANT_USER_ROLES.EDTIOR).ToList();
        }

        public User GetEditor(int id)
        {
            return context.Users.AsNoTracking().SingleOrDefault(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.EDTIOR);
        }

        public bool DeleteEditor(int id)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.EDTIOR);
            if (user == null)
                throw new AppException(HttpStatusCode.NotFound, "user Not Found");
            user.IsDeleted = true;
            context.SaveChanges();
            return true;
        }

        public bool ChangePostStatus(int postId, string statusId)
        {
            if (!IsPostStatusExist(statusId))
                throw new AppException(HttpStatusCode.BadRequest, "Invalid Status");
            var post = context.Posts.FirstOrDefault(x => x.Id == postId && !x.IsDeleted);
            if (post == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Post Not Found");
            }
            post.StatusId = statusId;
            context.SaveChanges();
            return true;
        }

        public List<Post> GetPosts(string status = "")
        {
            if (string.IsNullOrEmpty(status))
                return context.Posts.AsNoTracking().Where(x=>!x.IsDeleted).ToList();
            return context.Posts.AsNoTracking().Where(x => x.StatusId == status && !x.IsDeleted).ToList();
        }

        public List<Post> GetEditorPosts(int editorId)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId).ToList();
        }

        public List<Post> GetEditorPostsByStatus(int editorId, string status)
        {
            return context.Posts.AsNoTracking().Where(x => x.EditorId == editorId && x.StatusId == status).ToList();
        }

        public Post GetPostById(int id)
        {
            return context.Posts.AsNoTracking().SingleOrDefault(x => x.Id == id && !x.IsDeleted);
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

        public User UpdateAdmin(AdminViewModelU adminViewModelU)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == adminViewModelU.Id && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.ADMIN);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Admin not found");
            }
            user.FirstName = adminViewModelU.FirstName;
            user.LastName = adminViewModelU.LastName;
            if (user.Email != adminViewModelU.Email)
            {
                if (IsEmailExist(adminViewModelU.Email))
                    throw new AppException(HttpStatusCode.BadRequest, "Email Already Exist");
                else
                    user.Email = adminViewModelU.Email;
            }
            if (user.Phonenumber != adminViewModelU.Phonenumber)
            {
                if (IsPhonenumberExist(adminViewModelU.Phonenumber))
                    throw new AppException(HttpStatusCode.BadRequest, "Phone number Already Exist");
                else
                    user.Phonenumber = adminViewModelU.Phonenumber;
            }
            if (adminViewModelU.Photo != null)
                user.Photo = UploadImage(adminViewModelU.Photo);
            context.SaveChanges();
            return user;
        }
        public User ChangePassword(ChangePasswordViewModel viewModel)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == viewModel.UserId && !x.IsDeleted && x.Role == CONSTANT_USER_ROLES.ADMIN);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "Admin not found");
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