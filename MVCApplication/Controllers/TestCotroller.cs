using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCApplication.Views.Recipes;
using System.IO.Compression;

namespace MVCApplication.Controllers
{
    public class TestController : Controller
    {
        private readonly BiteBlissDBContext _context;

        public TestController(BiteBlissDBContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BufferedSingleFileUploadDbModel model)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.FileUpload.FormFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 20971520)
                    {
                        var file = new Recipe()
                        {
                            Banner = memoryStream.ToArray()
                        };

                        _context.Recipies.Add(file);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            return View();
        }
    }
}
