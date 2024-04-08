using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using ServiceLayer;

namespace MVCApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryManager _manager;

        public CategoriesController(CategoryManager manager)
        {
            _manager = manager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {

            return _manager.ReadAllAsync() != null ? 
                        View(await _manager.ReadAllAsync()) :
                        Problem("Entity set 'BiteBlissDBContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _manager.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var category = await _manager.ReadAsync(id, true, true);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _manager.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _manager.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var category = await _manager.ReadAsync(id, true, true);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _manager.UpdateAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _manager.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var category = await _manager.ReadAsync(id, true, true);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_manager.ReadAllAsync() == null)
            {
                return Problem("Entity set 'BiteBlissDBContext.Categories'  is null.");
            }
            var category = await _manager.ReadAsync(id, true, true);
            if (category != null)
            {
                await _manager.DeleteAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _manager.ReadAsync(id) != null;
        }
    }
}
