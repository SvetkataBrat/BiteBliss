using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Incorrect email!")]
        public string Email { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        // Add type of User (admin/normal)

        // Add img for pfp

        public List<Recipe> Recipes { get; set; }

        public User() 
        {
            this.Recipes = new List<Recipe>();
        }

        public User(string firstName_, string lastName_, string email_, DateOnly dateOfBirth_)
        {
            this.FirstName = firstName_;
            this.LastName = lastName_;
            this.Email = email_;
            this.DateOfBirth = dateOfBirth_;
            this.Recipes = new List<Recipe>();
        }
    }
}
