using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using MVCApplication.Models;
using System.Diagnostics;

namespace MVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BiteBlissDBContext _context;

        public HomeController(ILogger<HomeController> logger, BiteBlissDBContext context)
        {
            _logger = logger;
            _context = context;
        }

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