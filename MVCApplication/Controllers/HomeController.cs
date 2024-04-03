using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCApplication.Managers;
using MVCApplication.Models;
using SendGrid;
using System.Diagnostics;

namespace MVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BiteBlissDBContext _context;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, BiteBlissDBContext context, SignInManager<User> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            FIlteredRecipes model = new FIlteredRecipes();
            if (model.CategoryForRecipes == null)
            {
                return View(UpdateModel(model));
            }
            model = UpdateModel(model);
            model.Recipes = await model.GetFilteredSearchAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(FIlteredRecipes model)
        {
            if (model.CategoryForRecipes == null)
            {
                return View(UpdateModel(model));
            }
            model = UpdateModel(model);
            model.Recipes = await model.GetFilteredSearchAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            Response res = await EmailSenderManager.SendEmailFromContactsPage(model.Email, model.Username, model.Subject, model.Body);
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SuccessfulContact));
            }
            else
            {
                return RedirectToAction(nameof(UnSuccessfulContact));
            }
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> SuccessfulContact()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> UnSuccessfulContact()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> Filter(FIlteredRecipes model)
        {
            if (model.CategoryForRecipes == null)
            {
                return IndexAsync(model);
            }
            model = UpdateModel(model);
            model.Recipes = await model.GetFilteredSearchAsync();
            return IndexAsync(model);
        }*/

        private FIlteredRecipes UpdateModel(FIlteredRecipes model_)
        {
            model_.Recipes = _context.Recipies.ToList();
            model_.Categories = _context.Categories.ToList();
            return model_;
        }
    }
}