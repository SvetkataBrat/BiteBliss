using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CategoryManager
    {
        private readonly CategoryContext _context;

        public CategoryManager(CategoryContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category item) => await _context.CreateAsync(item);

        public async Task DeleteAsync(int key) => await _context.DeleteAsync(key);
        
        public async Task<ICollection<Category>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.ReadAllAsync(useNavigationalProperties, isReadOnly);
        
        public async Task<Category> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.ReadAsync(key, useNavigationalProperties, isReadOnly);
        
        public async Task UpdateAsync(Category item, bool useNavigationalProperties = false, bool isReadOnly = true) => await _context.UpdateAsync(item, useNavigationalProperties, isReadOnly);  
    }
}
