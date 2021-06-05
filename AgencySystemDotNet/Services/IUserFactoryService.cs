using AgencySystemDotNet.ViewModels;

using PressAgencyApp.Models;

namespace AgencySystemDotNet.Services
{
    public interface IUserFactoryService
    {
        BaseUserViewModel CreateUser(User user);
    }
}