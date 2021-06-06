using AgencySystemDotNet.Constants;
using AgencySystemDotNet.Services;

using AutoMapper;

using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Post;
using PressAgencyApp.ViewModels.PostQuestion;

using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AgencySystemDotNet.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly ILoginService loginService;
        private readonly IMapper mapper;

        public CustomerController(ILoginService loginService, ICustomerService customerService, IMapper mapper)
        {
            this.customerService = customerService;
            this.loginService = loginService;
            this.mapper = mapper;
        }

        // GET: Customer
        //[Route("Customer/Posts/{categoryId?}")]
        public ActionResult Posts(int? categoryId)
        {
            var posts = customerService.GetPosts(categoryId);
            var result = mapper.Map<List<PostViewModelR>>(posts);
            return View(result);
        }
        [HttpPost]
        public ActionResult SearchPosts(string searchValue)
        {
            var posts = customerService.SearchPosts(searchValue);
            var result = mapper.Map<List<PostViewModelR>>(posts);
            return View("Posts",result);
        }

        public ActionResult CustomerProfile()
        {
            int customerId = GetCustomerId();
            if (customerId == 0)
                return RedirectToAction("Posts", "Customer");
            var posts = customerService.GetCustomer(customerId);
            var result = mapper.Map<CustomerViewModelR>(posts);
            return View(result);
        }

        public ActionResult EditProfile()
        {
            int customerId = GetCustomerId();
            if (customerId == 0)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not Logined");
            var posts = customerService.GetCustomer(customerId);
            var result = mapper.Map<CustomerViewModelU>(posts);
            return View(result);
        }

        [HttpPost]
        public ActionResult EditProfile(CustomerViewModelU viewModelU)
        {
            int customerId = GetCustomerId();
            if (customerId == 0)
                return RedirectToAction("Posts", "Customer");
            viewModelU.Id = customerId;
            var posts = customerService.UpdateCustomer(viewModelU);
            var result = mapper.Map<CustomerViewModelU>(posts);
            return RedirectToAction("CustomerProfile");
        }
        

  
        public ActionResult Register( CustomerViewModelC viewModelC)
        {
            try
            {
                var user = customerService.CreateCustomer(viewModelC);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Email or password are incorrect";
                    return RedirectToAction("Posts");
                }
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ID, user.Id.ToString()));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.NAME, user.FirstName));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.EMAIL, user.Email));
                Request.Cookies.Add(new HttpCookie(CONSTANT_COOKIES_NAMES.ROLE, user.Role));

                return RedirectToAction("Posts", "Customer");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("Posts", "Customer");
            }
        }

        public ActionResult Post(int id)
        {
            int customerId = GetCustomerId();
            var posts = customerService.GetPost(id, customerId);
            var result = mapper.Map<PostViewModelR>(posts);
            return View(result);
        }

        //[HttpPost]
        public ActionResult LikePost(int id)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                int customerId = GetCustomerId();
                var post = customerService.LikePost(customerId, id);
                return RedirectToAction("Post", new { id = post });
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        public ActionResult AskQuestion(int id)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                int customerId = GetCustomerId();
                var post = customerService.GetPost(id);
                var postQuestionVm = new PostQuestionViewModelC
                {
                    CustomerId = customerId,
                    PostId = id,
                };
                return View(postQuestionVm);
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult AskQuestion(PostQuestionViewModelC postQuestionViewModelC)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                postQuestionViewModelC.CustomerId = GetCustomerId();
                var post = customerService.CreateQuestion(postQuestionViewModelC);
                return RedirectToAction("Post", new { id = post.PostId });
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        // [HttpPost]
        public ActionResult DislikePost(int id)
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetCustomerId();
            var post = customerService.DisLikePost(customerId, id);
            return RedirectToAction("Post", new { id = post });
        }

        public ActionResult ChangePassword()
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetCustomerId();
            return View(new ChangePasswordViewModel(customerId));
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetCustomerId();
            viewModel.Role = CONSTANT_USER_ROLES.CUSTOMER;
            var result = loginService.ChangePassword(viewModel);
            return RedirectToAction("Profile");
        }

      //  [HttpPost]
        public ActionResult SavePost(int id)
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetCustomerId();
            var result = customerService.SavePost(customerId, id);
            return RedirectToAction("Posts");
        }

        public ActionResult SavedPosts()
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetCustomerId();
            var result = customerService.GetSavedPosts(customerId);
            var posts = mapper.Map<List<PostViewModelR>>(result);
            return View(posts);
        }

        private bool Authorize()
        {
            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return false;
            var isAdmin = loginService.IsUserCustomer(int.Parse(id.Value));
            return isAdmin;
        }

        private int GetCustomerId()
        {
            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return 0;
            return int.Parse(id.Value);
        }
    }
}