using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using eKnjiznica.Data.Entities;

namespace eKnjiznica.Data.Repositories
{
    public interface IProductRepository
    {
        List<Product> FindAll();
        Task<List<Product>> FindAllAsync();
        Task<Product> FindOne(int id);
        void Add(Product product);
        Task SaveChangesAsync();
        void Update(Product product);
        void Remove(Product product);
        bool Any(int id);

        void SetCategoryRelationships(List<ProductCategory> relations);
        void RemoveCategoryRelatiionships(Product product);
        Task<List<ProductCategory>> FindAllInCategories(List<Category> filterCategories);

        //Task<List<Product>> FindByCategory(List<int> categoires);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Add(product);
        }

        public bool Any(int id)
        {
            return _context.Products.Any(product => product.Id == id);
        }

        public List<Product> FindAll()
        {
            return _context.Products.ToList();
        }

        public Task<List<Product>> FindAllAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<List<ProductCategory>> FindAllInCategories(List<Category> filterCategories)
        {
            return _context.ProductCategories.Include(pc => pc.Product).Where(pc => filterCategories.Contains(pc.Category)).ToListAsync();
        }

        public Task<Product> FindOne(int id)
        {
            return _context.Products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Product product)
        {
            _context.Remove(product);
        }

        public void RemoveCategoryRelatiionships(Product product)
        {
            foreach (var item in product.ProductCategories)
            {
                _context.Remove(item);
            }
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void SetCategoryRelationships(List<ProductCategory> relations)
        {
            foreach (var item in relations)
            {
                _context.Add(item);
            }
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }
    }

}
