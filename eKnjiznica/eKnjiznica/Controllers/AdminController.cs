﻿using eKnjiznica.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Controllers
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

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

        public Task<Product> FindOne(int id)
        {
             return _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Remove(Product product)
        {
            _context.Remove(product);
        }

        public Task SaveChangesAsync()
        {
             return _context.SaveChangesAsync();
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }
    }

    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            var allProducts = _productRepository.FindAll();

            return View(allProducts);
        }
    }
}
