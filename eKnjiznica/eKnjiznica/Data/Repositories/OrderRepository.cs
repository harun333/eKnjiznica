using eKnjiznica.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Repositories
{
    public interface IOrderRepository
    {
        List<Order> FindAll();
        Task<List<Order>> FindAllAsync();
        Task<Order> FindOne(int id);
        void Add(Order order);
        Task SaveChangesAsync();
        void Update(Order order);
        void Remove(Order order);
        bool Any(int id);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Order order)
        {
            _context.Add(order);
        }

        public bool Any(int id)
        {
            return _context.Categories.Any(order => order.Id == id);
        }

        public List<Order> FindAll()
        {
            return _context.Order.ToList();
        }

        public Task<List<Order>> FindAllAsync()
        {
            return _context.Order.ToListAsync();
        }

        public Task<Order> FindOne(int id)
        {
            return _context.Order
                   .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Order order)
        {
            _context.Remove(order);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Order order)
        {
            _context.Update(order);
        }
    }
}
