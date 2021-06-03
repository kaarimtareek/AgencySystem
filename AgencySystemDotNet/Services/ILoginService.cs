using PressAgencyApp.Models;
using PressAgencyApp.ViewModels;

namespace PressAgencyApp.Services
{
    public interface ILoginService
    {
        User Login(string email, string password);
        User ChangePassword(ChangePasswordViewModel viewModel);
        bool IsUserAdmin(int id);
        bool IsUserEditor(int id);
        bool IsUserCustomer(int id);
    }
}