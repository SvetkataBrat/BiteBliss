using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MVCApplication.Models;
using MVCApplication.Views.Recipes;

namespace MVCApplication.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly BiteBlissDBContext _context;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            BiteBlissDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public byte[] ProfilePicture_ { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var profilePic = user.ProfilePicture;

            Username = userName;
            Email = email;
            ProfilePicture_ = profilePic;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ModelState.Clear();
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (FileUpload.FormFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await FileUpload.FormFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 20971520)
                    {
                        user.ProfilePicture = memoryStream.ToArray();
                        User userFromDb = _context.Users.Find(user.Id);
                        userFromDb.ProfilePicture = user.ProfilePicture;
                        _context.Update(userFromDb);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
