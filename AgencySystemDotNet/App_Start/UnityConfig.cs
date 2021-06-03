using AutoMapper;

using PressAgencyApp.Models;
using PressAgencyApp.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Unity;
using Unity.AspNet.Mvc;

namespace AgencySystemDotNet.App_Start
{
    public class UnityConfig
    {
        public static UnityContainer Container = new UnityContainer();
        public static void RegisterComponents()
        {
            //var container = new UnityContainer();
            Container.RegisterType<IAdminService, AdminService>();
            Container.RegisterType<ILoginService, LoginService>();
            Container.RegisterType<ICustomerService, CustomerService>();
            Container.RegisterType<IEditorService, EditorService>();
            Container.RegisterType<IEditorService, EditorService>();
            MapperConfiguration config = AutoMapperConfig.Configure(); ;

            //build the mapper
            IMapper mapper = config.CreateMapper();

            Container.RegisterInstance<IMapper>(mapper);
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}