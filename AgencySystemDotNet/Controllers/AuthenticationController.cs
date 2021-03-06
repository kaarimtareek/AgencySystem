using AgencySystemDotNet.Constants;
using AgencySystemDotNet.Services;
using AgencySystemDotNet.ViewModels;
using AgencySystemDotNet.ViewModels.Admin;

using PressAgencyApp.Helpers;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;

using System;
using System.Web;
using System.Web.Mvc;

namespace AgencySystemDotNet.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ICustomerService customerService;
        private readonly IUserFactoryService userFactoryService;

        public AuthenticationController(ILoginService loginService, ICustomerService customerService, IUserFactoryService userFactoryService)
        {
            this.loginService = loginService;
            this.customerService = customerService;
            this.userFactoryService = userFactoryService;
        }

        // GET: Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                var user = loginService.Login(email, password);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Email or password are incorrect";
                    return View();
                }
                Response.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ID, user.Id.ToString()));
                Response.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.NAME, user.FirstName));
                Response.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.EMAIL, user.Email));
                Response.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ROLE, user.Role));
                var factoryUser = userFactoryService.CreateUser(user);
                return RouteAfterLogin(factoryUser);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }

        private ActionResult RouteAfterLogin(BaseUserViewModel model)
        {
            if (model is AdminViewModelR)
            {
                return RedirectToAction("Posts", "Admin");
            }
            if (model is EditorViewModelR)
            {
                return RedirectToAction("Posts", "Editor");
            }
            if (model is CustomerViewModelR)
            {
                return RedirectToAction("Posts", "Customer");
            }
            return HttpNotFound();
        }

       
        public ActionResult Logout()
        {
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Posts","Customer");
        }
    }
}