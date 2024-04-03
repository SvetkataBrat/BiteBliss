using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVCApplication.Areas.Identity.Pages.Account
{
    public class ErrorPageModel : PageModel
    {
        public string ErrorMessage { get; set; }

        public ErrorPageModel()
        {
            ErrorMessage = "Something went wrong with resetting your password!";
        }

        public void OnGet()
        {
            
        }
    }
}
