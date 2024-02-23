using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        [Required]
        public byte[] ProfilePicture { get; set; }

        public List<Recipe> Recipes { get; set; }

        public User() 
        {
            this.Recipes = new List<Recipe>();
        }

        public User(string username_, string email_)
        {
            this.UserName = username_;
            this.Email = email_;
            this.Recipes = new List<Recipe>();
            this.ProfilePicture = System.IO.File.ReadAllBytes("D:\\zz_Timi\\aa__SoftUni-C#\\Projects\\Exercises\\Tests\\BiteBliss\\MVCApplication\\DefaultImages\\ProfilePicture.jpg");
        }

        public override string ToString()
        {
            return $"UserName: {this.UserName} \nEmail: {this.Email}";
        }
    }
}
