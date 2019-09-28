using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eKnjiznica.Data;
using eKnjiznica.Data.Entities;
using eKnjiznica.Data.Repositories;
using eKnjiznica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKnjiznica.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public ActionResult Index()
        {
           
            return View(_context.Products.OrderByDescending(x=>x.Id).ToList());
        }
        public ActionResult AddToCart(int ?id)
        {
            Product p = _context.Products.Where(x => x.Id == id).SingleOrDefault();
            return View(p);
        }
       

        [HttpPost]
        public ActionResult AddToCart(Product pi, string qty, int id)
        {
            List<Cart> li = new List<Cart>();
            Product p = _context.Products.Where(x => x.Id == id).SingleOrDefault();
       
            Cart c = new Cart();
            c.ProductId = p.Id;
            c.price = p.Price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.ProductName = p.Name;
            li.Add(c);
            _context.Carts.Add(c);
            _context.SaveChanges();
            return RedirectToAction("CheckOut",new { id = c.Id });

        }
        public IActionResult CheckOut(int id)
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var client = _context.Clients.Where(x => x.applicationUserId == user.Id).FirstOrDefault();
            if (client.Credit > 0){ 
            var cart = _context.Carts.Include(x=>x.Product).Where(x => x.Id == id).FirstOrDefault();
            var clientCredit = client.Credit;
            var total = cart.qty * (int)cart.price;

            client.Credit = clientCredit - total;
            user.Credit = client.Credit;
            _context.SaveChanges();


            return View(cart);
            }
            else
            {
                return RedirectToAction("Index", "Products");
            }
    }
    }
}