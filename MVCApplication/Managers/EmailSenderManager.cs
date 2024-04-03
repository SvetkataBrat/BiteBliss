using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace MVCApplication.Managers
{
    public class EmailSenderManager : IEmailSender
    {
        public static async Task<Response> SendEmailAsyncTask(string email, string username, string subject, string body)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("svetomirkoev123@gmail.com", "BiteBliss Admin");

                var to = new EmailAddress(email, "EmailReciever");

                var htmlContent = $"<div style=\"display:flex; widht: 10%;\">" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">User: {username}  /  {email}</h3>" +
                                    $"<hr>" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">Sibject: {subject}</h3>" +
                                    $"</div>" +
                                    $"<hr>" +
                                    $"<h3 style=\"text-align: center;\">{body}</h3>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }

        public static async Task<Response> SendEmailFromContactsPage(string email, string username, string subject, string body)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("svetomirkoev123@gmail.com", "BiteBliss Admin");

                var to = new EmailAddress("monskipx@gmail.com", "EmailReciever");

                var htmlContent = $"<div style=\"display:flex; widht: 10%;\">" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">User: {username}  /  {email}</h3>" +
                                    $"<hr>" +
                                    $"<h3 style=\"font-weight: 100; width: 50%; text-align: center;\">Sibject: {subject}</h3>" +
                                    $"</div>" +
                                    $"<hr>" +
                                    $"<h3 style=\"text-align: center;\">{body}</h3>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
