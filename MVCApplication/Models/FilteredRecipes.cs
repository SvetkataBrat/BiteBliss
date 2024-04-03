using BusinessLayer;

namespace MVCApplication.Models
{
    public class FIlteredRecipes
    {
        public Category CategoryForRecipes { get; set; }

        public List<Recipe> Recipes { get; set; }

        public List<Category> Categories { get; set; }

        public async Task<List<Recipe>> GetFilteredSearchAsync()
        {
            if (this.CategoryForRecipes.Name == "Default")
            {
                return Recipes;
            }
            List<Recipe> recipes = new List<Recipe>();
            recipes = Recipes.Where(x => x.Category.Name == CategoryForRecipes.Name).ToList();
            return recipes;
        }

        public FIlteredRecipes() 
        {
            CategoryForRecipes = new Category 
            {
                Name = "Default"
            };
        }
    }
}
