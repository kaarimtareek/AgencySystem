using AgencySystemDotNet.ViewModels;
using AgencySystemDotNet.ViewModels.Admin;

using AutoMapper;

using PressAgencyApp.Constants;
using PressAgencyApp.Models;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgencySystemDotNet.Services
{
    public class UserFactoryService : IUserFactoryService
    {
        private readonly IMapper mapper;

        public UserFactoryService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public BaseUserViewModel CreateUser(User user)
        {
            if (user.Role == CONSTANT_USER_ROLES.ADMIN)
                return mapper.Map<AdminViewModelR>(user);
            else if (user.Role == CONSTANT_USER_ROLES.EDTIOR)
                return mapper.Map<EditorViewModelR>(user);
            else if (user.Role == CONSTANT_USER_ROLES.CUSTOMER)
                return mapper.Map<CustomerViewModelR>(user);
            else return null;
        }
    }
}