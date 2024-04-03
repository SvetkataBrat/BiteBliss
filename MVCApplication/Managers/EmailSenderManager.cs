﻿using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace MVCApplication.Managers
{
    public class EmailSenderManager : IEmailSender
    {
        public async Task SendEmailAsyncTask(string email, string subject, string htmlMessage)
        {
            string myMail = "apikey";
            string myPassword = "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";

            string email_ = "svetomirkoev123@gmail.com";

            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 25);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(myMail, myPassword);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(email_);
                mailMessage.To.Add("monskipx@gmail.com");
                mailMessage.Subject = subject;
                mailMessage.Body = htmlMessage;

                smtpClient.Send(mailMessage);
            }
            catch(Exception ex)
            {
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
