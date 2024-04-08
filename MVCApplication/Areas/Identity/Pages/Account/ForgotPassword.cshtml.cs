// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MVCApplication.Controllers;
using MVCApplication.Managers;
using static System.Net.Mime.MediaTypeNames;

namespace MVCApplication.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailSenderManager _emailSender;
        private readonly BiteBlissDBContext _context;

        private readonly string allase64;

        private readonly int codeMaxLength;


        public ForgotPasswordModel 
        (
            UserManager<User> userManager, 
            EmailSenderManager emailSender, 
            BiteBlissDBContext biteBlissDBContext,
            HomeController controller
        )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            codeMaxLength = 20;
            _context = biteBlissDBContext;
            allase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        private string GenerateCustomCodeForPasswordReset()
        {
            string code_ = "";

            Random r = new Random();

            for (int i = 0; i < codeMaxLength; i++)
            {
                code_ += allase64[r.Next(0, allase64.Length-1)];
            }

            return code_;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    string msg = "Coudn\'t find user with that email!";
                    return RedirectToPage("./ErrorPage", new { ErrorMessage = msg });
                }

                var code = GenerateCustomCodeForPasswordReset();
                user.SecurityStamp = code;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                
                var callbackUrl = Url.Page(
                     "/Account/ResetPass",
                     pageHandler: null,
                     values: new { area = "Identity", code },
                     protocol: Request.Scheme);

                

                var result = await EmailSenderManager.SendEmailAsyncTask(
                    user.Email,
                    user.UserName,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                else
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }
            return Page();
        }
    }
}
