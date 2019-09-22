using eKnjiznica.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Repositories
{
    public interface IClientRepository
    {
        List<Client> FindAll();
        Task<List<Client>> FindAllAsync();
        Task<Client> FindOne(int id);
        void Add(Client category);
        Task SaveChangesAsync();
        void Update(Client category);
        void Remove(Client category);
        bool Any(int id);
    }
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Client client)
        {
            _context.Add(client);
        }

        public bool Any(int id)
        {
            return _context.Clients.Any(client => client.Id == id);
        }

        public List<Client> FindAll()
        {
            return _context.Clients.ToList();
        }

        public Task<List<Client>> FindAllAsync()
        {
            return _context.Clients.ToListAsync();
        }

        public Task<Client> FindOne(int id)
        {
            return _context.Clients
                   .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Client client)
        {
            _context.Remove(client);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Client client)
        {
            _context.Update(client);
        }
    }
}
