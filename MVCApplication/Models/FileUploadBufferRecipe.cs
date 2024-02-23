using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using MVCApplication.Views.Recipes;

namespace MVCApplication.Models
{
    public class FileUploadBufferRecipe
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public Recipe curRecipe { get; set; }   
    }
}
