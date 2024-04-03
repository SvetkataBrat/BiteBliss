using BusinessLayer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ServiceLayer.ResultSets;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;

namespace MVCApplication.Areas.Identity.Pages.Account
{
    public class ResetPassModel : PageModel
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IdentityManager _identityManager;
        private readonly ILogger<User> _loggger;

        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPassModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IdentityManager identityManager, 
            ILogger<User> loggger,
            IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _identityManager = identityManager;
            _loggger = loggger;
            _passwordHasher = passwordHasher;
        }

        public class InputModel
        {
            [Required]
            public string Code { get; set;}

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string codeForVerification { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(InputModel model)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {

                User user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    return NotFound();
                }

                if (model.Code != user.SecurityStamp)
                {
                    AccessDeniedModel accessDenied = new AccessDeniedModel();
                    return accessDenied.BadRequest();
                }

                var removePassRes = await _signInManager.UserManager.RemovePasswordAsync(user);

                if (removePassRes.Succeeded)
                {
                    var addPassRes = await _signInManager.UserManager.AddPasswordAsync(user, Input.Password);
                    if (addPassRes.Succeeded)
                    {
                        return RedirectToPage("./Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", addPassRes.Errors.FirstOrDefault().ToString());
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError("", removePassRes.Errors.FirstOrDefault().ToString());
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
