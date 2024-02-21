using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public byte[] Banner { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title is too long!")]
        public string Title { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Descritpion is too long!")]
        public string Description { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The list of instructions is too long!")]
        public string Instructions { get; set; }

        [Required]
        public DateOnly DateOfPublish { get; set; }

        [Required]
        public Category @Category { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        public User @User { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public Recipe() { }

        public Recipe(string title_, string description_, string instructions_, DateOnly dateOfPublish_, Category category_, User user_)
        {
            this.Title = title_;
            this.Description = description_;
            this.Instructions = instructions_;
            this.DateOfPublish = dateOfPublish_;
            this.Category = category_;
            this.User = user_;
            this.Banner = System.IO.File.ReadAllBytes("\\MVCApplication\\bin\\Debug\\net6.0\\Images\\DefaultRecipeBanner.png");
        }

    }
}
