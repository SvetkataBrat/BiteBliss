using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCApplication.Views.Recipes
{
    public class BufferedSingleFileUploadDbModel
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }
    }


    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "ModelBanner")]
        public IFormFile FormFile { get; set; }
    }
}
