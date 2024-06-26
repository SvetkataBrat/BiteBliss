﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using MVCApplication.Views.Recipes;
using MVCApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using ServiceLayer;

namespace MVCApplication.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeManager _manager;
        private readonly BiteBlissDBContext _context;

        public RecipesController(RecipeManager manager)
        {
            _manager = manager;
        }

        // GET: Recipes
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Index()
        {
            return View(await _manager.ReadAllAsync(true, true));
        }

        // GET: Recipes/Details/5
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _manager.ReadAsync(id) == null)
            {
                return NotFound();
            }
            
            var recipe = await _manager.ReadAsync(id, true, true);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize(Roles = "Administrator, User")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Create(FileUploadBufferRecipe model)
        {
            model.curRecipe.User = await _context.Users.FindAsync(model.curRecipe.UserId);
            model.curRecipe.Category = await _context.Categories.FindAsync(model.curRecipe.CategoryId);
            Recipe recipe = new Recipe(model.curRecipe.Title, model.curRecipe.Description, model.curRecipe.Instructions, model.curRecipe.Ingredients,  model.curRecipe.Category, model.curRecipe.User);
            recipe.DateOfPublish = DateTime.Now;
            ModelState.Clear();
            if (model.FileUpload == null)
            {
                model.FileUpload = new BufferedSingleFileUploadDb();
                var stream = System.IO.File.OpenRead("./DefaultImages/defaultRecipeBanner.png");
                model.FileUpload.FormFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
            }
            if (TryValidateModel(model))
            {
                using (var memoryStream = new MemoryStream())
                {   
                    await model.FileUpload.FormFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 20971520)
                    {
                        recipe.Banner = memoryStream.ToArray();
                        await _manager.CreateAsync(recipe);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", recipe.UserId);
            return View(model);
        }

        // GET: Recipes/Edit/5
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Add authorization for the spcific user
            if (id == null || _context.Recipies == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipies.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", recipe.UserId);
            FileUploadBufferRecipe model = new FileUploadBufferRecipe();
            model.curRecipe = recipe;
            return View(model);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Banner,Title,Description,Instructions,DateOfPublish,CategoryId,UserId")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _manager.UpdateAsync(recipe, true, true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", recipe.UserId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Recipies == null)
            {
                return NotFound();
            }

            var recipe = await _manager.ReadAsync(id, true, true);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [Authorize(Roles = "Administrator, User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_manager.ReadAllAsync() == null)
            {
                return Problem("Entity set 'BiteBlissDBContext.Recipies'  is null.");
            }

            var recipe = await _manager.ReadAsync(id, true, true);

            if (recipe != null)
            {
                await _manager.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
