using eKnjiznica.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> FindAll();
        Task<List<Category>> FindAllAsync();
        Task<Category> FindOne(int id);
        void Add(Category category);
        Task SaveChangesAsync();
        void Update(Category category);
        void Remove(Category category);
        bool Any(int id);
    }
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Add(category);
        }

        public bool Any(int id)
        {
            return _context.Categories.Any(category => category.Id == id);
        }

        public List<Category> FindAll()
        {
            return _context.Categories.ToList();
        }

        public Task<List<Category>> FindAllAsync()
        {
            return _context.Categories.ToListAsync();
        }

        public Task<Category> FindOne(int id)
        {
            return _context.Categories
                   .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Category category)
        {
            _context.Remove(category);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }
    
    }
}
