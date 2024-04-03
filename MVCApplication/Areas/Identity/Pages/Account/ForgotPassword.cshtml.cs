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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MVCApplication.Managers;
using static System.Net.Mime.MediaTypeNames;

namespace MVCApplication.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailSenderManager _emailSender;

        private readonly int codeMaxLength;
        private readonly Byte[] _privateKey; // NOTE: You should use a private-key that's a LOT longer than just 4 bytes.
        private readonly TimeSpan _passwordResetExpiry;
        private static readonly Byte _version = 1; // Increment this whenever the structure of the message changes.


        public ForgotPasswordModel(UserManager<User> userManager, EmailSenderManager emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            codeMaxLength = 10;
            _privateKey = new Byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
            _passwordResetExpiry = TimeSpan.FromMinutes(5);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        

        public String CreatePasswordResetHmacCode(string userId)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(userId);

            Byte[] message = Enumerable.Empty<Byte>()
                .Append(ForgotPasswordModel._version)
                .Concat(bytes)
                .Concat(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()))
                .ToArray();

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                Byte[] hash = hmacSha256.ComputeHash(buffer: message, offset: 0, count: message.Length);

                Byte[] outputMessage = message.Concat(hash).ToArray();
                String outputCodeB64 = Convert.ToBase64String(outputMessage);
                String outputCode = outputCodeB64.Replace('+', '-').Replace('/', '_');
                return outputCode;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                /*if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                */
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713



                var code = CreatePasswordResetHmacCode(user.Id);

                var callbackUrl = Url.Page(
                     "/Account/ResetPass",
                     pageHandler: null,
                     values: new { area = "Identity", code },
                     protocol: Request.Scheme);

                /*await _emailSender.SendEmailAsyncTask(
                    "monskipx@gmail.com",
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                
                */
                return RedirectToPage("./ForgotPasswordConfirmation");

                /*
                string code = ""; 
                Random r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    code += r.Next(0,100).ToString()[0];
                }
                var textBytes = System.Text.Encoding.UTF8.GetBytes(code);
                code = System.Convert.ToBase64String(textBytes);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsyncTask(
                    "monskipx@gmail.com",
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");*/
            }

            return Page();
        }
    }
}
