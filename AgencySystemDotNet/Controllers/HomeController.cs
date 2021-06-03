using AgencySystemDotNet.Constants;

using PressAgencyApp.Services;
using PressAgencyApp.ViewModels.PostQuestion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AgencySystemDotNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService customerService;

        public HomeController()
        {

        }
        public HomeController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        //home view for customers , to see the posts
        public ActionResult Index()
        {
          
           
            return View();
        }
        //to see specific post
        public ActionResult Post(int postId)
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        //to ask question
        public ActionResult CreateQuestion([Bind] PostQuestionViewModelC viewModelC)
        {

           var id = Request.Cookies.Get(CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return View("UnAuthorized");
            return View();
        }
    }
}