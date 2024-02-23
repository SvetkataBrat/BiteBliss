using Microsoft.AspNetCore.Identity.UI.Services;

namespace MVCApplication.Managers
{
    public class EmailSenderManager : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
