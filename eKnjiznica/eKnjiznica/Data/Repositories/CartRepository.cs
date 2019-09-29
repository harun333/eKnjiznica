using eKnjiznica.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Repositories
{
    public interface ICartRepository
    {
        void RemoveProductRelated(int productId);
    }

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void RemoveProductRelated(int productId)
        {
            var cartItemsToRemoveProductsFrom = _context.Carts.Where(c => c.ProductId == productId).ToList();

            foreach (var cart in cartItemsToRemoveProductsFrom)
            {
                _context.Remove(cart);
            }
        }
    }
}
