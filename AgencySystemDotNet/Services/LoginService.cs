using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AgencySystemDotNet.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext context;

        public LoginService(AppDbContext context)
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
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public User ChangePassword(ChangePasswordViewModel viewModel)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == viewModel.UserId && !x.IsDeleted&& x.Role == viewModel.Role);
            if (user == null)
            {
                throw new AppException(HttpStatusCode.NotFound, "User not found");
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
        public User Login(string email, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            if (!VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                throw new AppException(HttpStatusCode.BadRequest, "Incorrect email or password");
            return user;
        }
        public bool IsUserAdmin(int id)
        {
            return context.Users.Any(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.ADMIN);
        }
        public bool IsUserCustomer(int id)
        {
            return context.Users.Any(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.CUSTOMER);
        }
        public bool IsUserEditor(int id)
        {
            return context.Users.Any(x => x.Id == id && x.Role == CONSTANT_USER_ROLES.EDTIOR);
        }
    }
}
