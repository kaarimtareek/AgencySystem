using AgencySystemDotNet.ViewModels.Admin;

using AutoMapper;

using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.Services;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgencySystemDotNet.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILoginService loginService;
        private readonly IAdminService adminService;
        private readonly IMapper mapper;

        // GET: Admin
        public AdminController(ILoginService loginService, IAdminService adminService, IMapper mapper)
        {
            this.loginService = loginService;
            this.adminService = adminService;
            this.mapper = mapper;
        }
       
        public ActionResult AdminProfile()
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            int adminId = GetAdminId();
            var posts = adminService.GetAdmin(adminId);
            var result = mapper.Map<AdminViewModelR>(posts);
            return View(result);
        }
        [Route("admin/posts/{status?}")]
        public ActionResult Posts(string status )
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            var posts = adminService.GetPosts(status);
            var result = mapper.Map<List<PostViewModelR>>(posts);
            return View(result);
        }

        
        public ActionResult Post(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetPostById(id);
                var result = mapper.Map<PostViewModelR>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        [Route("admin/Approvepost/{id}")]
        public ActionResult ApprovePost(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.ChangePostStatus(id,CONSTANT_POST_STATUS.APPROVED);
                //var result = mapper.Map<PostViewModelR>(posts);
                return RedirectToAction("posts");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
  
        public ActionResult DeletePost(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.DeletePost(id);
                //var result = mapper.Map<PostViewModelR>(posts);
                return RedirectToAction("posts");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpStatusCodeResult(e.StatusCode,e.Message);
            }
        }
        [Route("admin/RejectPost/{id}")]
        public ActionResult RejectPost(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.ChangePostStatus(id,CONSTANT_POST_STATUS.REJECTED);
                //var result = mapper.Map<PostViewModelR>(posts);
                return RedirectToAction("posts");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult EditorPosts(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetEditorPosts(id);
                var result = mapper.Map<List<PostViewModelR>>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult Editor(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetEditor(id);
                var result = mapper.Map<EditorViewModelR>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult DeleteEditor(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.DeleteEditor(id);
                //var result = mapper.Map<EditorViewModelR>(posts);
                return RedirectToAction("Editors");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        
        public ActionResult Editors()
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetEditors();
                var result = mapper.Map<List<EditorViewModelR>>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult Customers()
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetCustomers();
                var result = mapper.Map<List<CustomerViewModelR>>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult Customer(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetCustomer(id);
                var result = mapper.Map<CustomerViewModelR>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult EditCustomer(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.GetCustomer(id);
                var result = mapper.Map<CustomerViewModelR>(posts);
                return View(result);
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        
        public ActionResult CreateCustomer()
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();

                return View(new CustomerViewModelC());
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpStatusCodeResult(e.StatusCode,e.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateCustomer(CustomerViewModelC viewModelU)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.CreateCustomer(viewModelU);
                var result = mapper.Map<CustomerViewModelR>(posts);
                return RedirectToAction("Customers");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpStatusCodeResult(e.StatusCode,e.Message);
            }
        }
        public ActionResult DeletCustomer(int id)
        {
            try
            {
                if (!Authorize())
                    return new HttpUnauthorizedResult();
                var posts = adminService.DeleteCustomer(id);
                //var result = mapper.Map<CustomerViewModelR>(posts);
                return RedirectToAction("Customers");
            }
            catch (AppException e)
            {
                ViewBag.ErrorMessage = e.Message;
                return new HttpNotFoundResult();
            }
        }
        public ActionResult ChangePassword()
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            int customerId = GetAdminId();
            return View(new ChangePasswordViewModel(customerId));
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            int customerId = GetAdminId();
            viewModel.Role = CONSTANT_USER_ROLES.ADMIN;
            var result = loginService.ChangePassword(viewModel);
            return RedirectToAction("AdminProfile");
        }
        public ActionResult EditProfile()
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            int customerId = GetAdminId();
           
            var posts = adminService.GetAdmin(customerId);
            var result = mapper.Map<AdminViewModelU>(posts);
            return View(result);
        }
        [HttpPost]
        public ActionResult EditProfile(AdminViewModelU viewModelU)
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();
            int customerId = GetAdminId();
            viewModelU.Id = customerId;
            var posts = adminService.UpdateAdmin(viewModelU);
            var result = mapper.Map<AdminViewModelU>(posts);
            return RedirectToAction("AdminProfile");
        }

        private bool Authorize()
        {

            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return false;
            var isAdmin = loginService.IsUserAdmin(int.Parse(id.Value));
            return isAdmin;
        }
        private int GetAdminId()
        {

            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return 0;
            return int.Parse(id.Value);
        }

    }
}