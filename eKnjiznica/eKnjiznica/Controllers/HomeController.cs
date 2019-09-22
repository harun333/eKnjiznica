using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eKnjiznica.Models;
using Microsoft.AspNetCore.Identity;
using eKnjiznica.Data;

namespace eKnjiznica.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)

        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //If there is already some data in DB we'll return
            if (_context.Users.Any())
                return View();

            //Ensure that the database exists 
            _context.Database.EnsureCreated();
                try
                {
                    var user = new ApplicationUser { UserName = "Admin", Email = "Admin@hotmail.com" };
                    var result = await _userManager.CreateAsync(user, "Test123$");

                    if (!result.Succeeded)
                    {
                        throw new Exception("Error");
                    }
                    // We'll read posts data from JSON file located in JSONData folder 
                }
                catch (Exception)
                {
                    throw;
                }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
       
    }
}
