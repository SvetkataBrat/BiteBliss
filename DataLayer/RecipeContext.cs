using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RecipeContext : IDb<Recipe, int>
    {
        private BiteBlissDBContext context;

        public RecipeContext(BiteBlissDBContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(Recipe item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Recipe>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Recipe item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            throw new NotImplementedException();
        }
    }
}
