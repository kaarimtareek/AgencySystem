using AgencySystemDotNet.Services;
using AgencySystemDotNet.ViewModels.Editor;
using AgencySystemDotNet.ViewModels.PostQuestion;

using AutoMapper;

using PressAgencyApp.Constants;
using PressAgencyApp.Helpers;
using PressAgencyApp.ViewModels;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;
using PressAgencyApp.ViewModels.PostQuestion;

using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace AgencySystemDotNet.Controllers
{
    public class EditorController : Controller
    {
        private readonly ILoginService loginService;
        private readonly IEditorService editorService;
        private readonly IMapper mapper;

        public EditorController(ILoginService loginService, IEditorService editorService, IMapper mapper)
        {
            this.loginService = loginService;
            this.editorService = editorService;
            this.mapper = mapper;
        }

        // GET: Editor
        public ActionResult Posts()
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            var posts = editorService.GetEditorPosts(GetEditorId());
            var result = mapper.Map<List<PostViewModelR>>(posts);

            return View(result);
        }

        public ActionResult Post(int id)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                var post = editorService.GetPostById(id);
                var result = mapper.Map<PostViewModelR>(post);
                return View(result);
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult CreatePost(PostViewModelC viewModelC)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                viewModelC.EditorId = GetEditorId();
                var post = editorService.CreatePost(viewModelC);
                var result = mapper.Map<PostViewModelR>(post);
                return RedirectToAction("Posts");
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        public ActionResult CreatePost()
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");

                return View(new PostViewModelC());
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult EditPost(PostViewModelU viewModelU)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                var post = editorService.UpdatePost(viewModelU);
                var result = mapper.Map<PostViewModelR>(post);
                return RedirectToAction("Posts");
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                var post = editorService.DeletePost(id);
                //var result = mapper.Map<PostViewModelR>(post);
                return RedirectToAction("Posts");
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        public ActionResult EditPost(int id)
        {
            try
            {
                if (!Authorize())
                    return RedirectToAction("Posts", "Customer");
                var post = editorService.GetPostById(id);
                var result = mapper.Map<PostViewModelU>(post);
                return View(result);
            }
            catch (AppException e)
            {
                return new HttpStatusCodeResult(e.StatusCode, e.Message);
            }
        }

        public ActionResult ChangePassword()
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetEditorId();
            return View(new ChangePasswordViewModel(customerId));
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int customerId = GetEditorId();
            viewModel.Role = CONSTANT_USER_ROLES.EDTIOR;
            var result = loginService.ChangePassword(viewModel);
            return RedirectToAction("EditorProfile");
        }

        public ActionResult EditProfile()
        {
            if (!Authorize())
                return new HttpUnauthorizedResult();

            int customerId = GetEditorId();
            var posts = editorService.GetEditor(customerId);
            var result = mapper.Map<EditorViewModelU>(posts);
            return View(result);
        }

        [HttpPost]
        public ActionResult EditProfile(EditorViewModelU viewModelU)
        {
            if (!Authorize())
            {
                return RedirectToAction("Posts", "Customer");
            }
            int customerId = GetEditorId();
            if (customerId == 0)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not Logined");
            viewModelU.Id = customerId;
            var posts = editorService.UpdateEditor(viewModelU);
            var result = mapper.Map<EditorViewModelU>(posts);
            return RedirectToAction("EditorProfile");
        }

        public ActionResult AnswerQuestion(int id)
        {
            if (!Authorize())
            {
                return RedirectToAction("Posts", "Customer");
            }
            var question = editorService.GetPostQuestion(id);
            var result = mapper.Map<PostQuestionViewModelU>(question);
            return View(result);
        }
        public ActionResult EditorProfile()
        {
            if (!Authorize())
                return RedirectToAction("Posts", "Customer");
            int adminId = GetEditorId();
            var posts = editorService.GetEditor(adminId);
            var result = mapper.Map<EditorViewModelR>(posts);
            return View(result);
        }
        [HttpPost]
        public ActionResult AnswerQuestion(PostQuestionViewModelU viewModelU)
        {
            if (!Authorize())
            {
                return RedirectToAction("Posts", "Customer");
            }
            var question = editorService.AnswerQuestion(viewModelU.Id, viewModelU.Answer);
            var result = mapper.Map<PostQuestionViewModelR>(question);
            return RedirectToAction("Post", new { id = result.PostId });
        }

        private bool Authorize()
        {
            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id == null)
                return false;
            var isAdmin = loginService.IsUserEditor(int.Parse(id.Value));
            return isAdmin;
        }

        private int GetEditorId()
        {
            var id = Request.Cookies.Get(Constants.CONSTANT_COOKIES_NAMES.ID);
            if (id != null)
                return int.Parse(id.Value);
            return 0;
        }
    }
}