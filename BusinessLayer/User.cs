using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Incorrect email!")]
        public override string Email { get; set; }

        [Required]
        public byte[] ProfilePicture { get; set; }

        public List<Recipe> Recipes { get; set; }

        public User() 
        {
            this.Recipes = new List<Recipe>();
        }

        public User(string firstName_, string lastName_, string username_, string email_)
        {
            this.FirstName = firstName_;
            this.LastName = lastName_;
            this.UserName = username_;
            this.Email = email_;
            this.Recipes = new List<Recipe>();
        }
    }
}
