using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CategoryContext : IDb<Category, int>
    {
        private BiteBlissDBContext dBContext;

        public CategoryContext(BiteBlissDBContext context_)
        {
            this.dBContext = context_;
        }

        public async Task CreateAsync(Category item)
        {
            try
            {
                dBContext.Categories.Add(item);
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
                Category categoryFromDb = await ReadAsync(key, false, false);
                dBContext.Categories.Remove(categoryFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Category>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                return dBContext.Categories.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                return dBContext.Categories.Find(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Category item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Category categoryFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (categoryFromDb == null) { await CreateAsync(item); }

                dBContext.Entry(categoryFromDb).CurrentValues.SetValues(item);

                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
