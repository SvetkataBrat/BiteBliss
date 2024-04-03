using System.ComponentModel.DataAnnotations;

namespace MVCApplication.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

    }
}
