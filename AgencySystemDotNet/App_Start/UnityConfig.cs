using AgencySystemDotNet.Services;

using AutoMapper;

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
            Container.RegisterType<IUserFactoryService, UserFactoryService>();
            MapperConfiguration config = AutoMapperConfig.Configure(); 

            //build the mapper
            IMapper mapper = config.CreateMapper();

            Container.RegisterInstance<IMapper>(mapper);
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}