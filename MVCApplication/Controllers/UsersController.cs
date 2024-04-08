using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;
using System.Data;

namespace MVCApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly BiteBlissDBContext _context;
        private readonly IdentityManager _manager;

        public UsersController(BiteBlissDBContext context, IdentityManager manager)
        {
            _context = context;
            _manager = manager;
        }

        // GET: Foods
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var svetomirDbContext = _context.Users.Include(r => r.Recipes);
            return View(await svetomirDbContext.ToListAsync());
        }

        // GET: Foods/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _manager.ReadUserAsync(id, true);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Foods/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _manager.ReadUserAsync(id, true);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Foods"] = new SelectList(_context.Users, "Id", "Recipies", user.Recipes);
            return View(user);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    User userFromDb = await _manager.ReadUserAsync(id, true);
                    userFromDb.UserName = user.UserName;
                    userFromDb.Email = user.Email;
                    userFromDb.ProfilePicture = user.ProfilePicture;
                    await _manager.UpdateUserAsync(id, userFromDb.UserName, userFromDb.Email);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Foods/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _manager.ReadUserAsync(id, true);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Foods/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DiqnDbContext.Users'  is null.");
            }
            var user = await _manager.ReadUserAsync(id, true);

            if (user != null)
            {
                await _manager.DeleteUserByNameAsync(user.UserName);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
