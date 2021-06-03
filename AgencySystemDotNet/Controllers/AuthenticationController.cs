using AgencySystemDotNet.Constants;

using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Services;
using PressAgencyApp.ViewModels.Customer;

using System;
using System.Web;
using System.Web.Mvc;

namespace AgencySystemDotNet.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginService loginService;
        private readonly ICustomerService customerService;

        public AuthenticationController(ILoginService loginService, ICustomerService customerService)
        {
            this.loginService = loginService;
            this.customerService = customerService;
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
                if(user.Role == CONSTANT_USER_ROLES.ADMIN)
                return RedirectToAction("Posts", "Admin");
                if (user.Role == CONSTANT_USER_ROLES.EDTIOR)
                return RedirectToAction("Posts", "Editor");
                
                return RedirectToAction("Posts", "Customer");

            }

            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
            }



        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind] CustomerViewModelC viewModelC)
        {
            try
            {
                var user = customerService.CreateCustomer(viewModelC);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Email or password are incorrect";
                    return View();
                }
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ID, user.Id.ToString()));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.NAME, user.FirstName));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.EMAIL, user.Email));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ROLE, user.Role));

                return RedirectToAction("Index", "HomeController");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
    }
}