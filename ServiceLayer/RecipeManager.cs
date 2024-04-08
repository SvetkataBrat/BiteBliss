using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class RecipeManager
    {
        private readonly RecipeContext _context;

        public RecipeManager(RecipeContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Recipe item) => await _context.CreateAsync(item);

        public async Task DeleteAsync(int key) => await _context.DeleteAsync(key);

        public async Task<ICollection<Recipe>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.ReadAllAsync(useNavigationalProperties, isReadOnly);

        public async Task<Recipe> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.ReadAsync(key, useNavigationalProperties, isReadOnly);

        public async Task UpdateAsync(Recipe item, bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.UpdateAsync(item, useNavigationalProperties, isReadOnly);
    }
}
