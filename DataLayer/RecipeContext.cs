using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RecipeContext : IDb<Recipe, int>
    {
        private BiteBlissDBContext dBContext;

        public RecipeContext(BiteBlissDBContext context)
        {
            this.dBContext = context;
        }

        public async Task CreateAsync(Recipe item)
        {
            try
            {
                dBContext.Recipies.Add(item);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Recipe recipeFromDb = await ReadAsync(key, true, false);
                dBContext.Recipies.Remove(recipeFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Recipe>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                List<Recipe> query = await dBContext.Recipies.ToListAsync();
                if (useNavigationalProperties)
                {
                    query = await dBContext.Recipies
                            .Include(x => x.Category)
                            .Include(x => x.User)
                            .ToListAsync();
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Recipe> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Recipe searchRecipe = await dBContext.Recipies.FindAsync(key);
                if (useNavigationalProperties)
                {
                    searchRecipe = await dBContext.Recipies
                            .Include(x => x.Category)
                            .Include(x => x.User).FirstOrDefaultAsync(x => x.Id == key);
                }
                return searchRecipe;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Recipe item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Recipe recipeFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (recipeFromDb == null) { await CreateAsync(item); }

                dBContext.Entry(recipeFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    recipeFromDb.User = item.User;
                }

                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
