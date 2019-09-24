//using eKnjiznica.Data;
//using eKnjiznica.Models;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace eKnjiznica.Helpers
//{
//    public class DbSeeder
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly ApplicationDbContext _context;
//        public DbSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager)

//        {
//            _userManager = userManager;
//            _context = context;
//        }
//        public async Task SeedDataAsync()
//        {
//            // If there is already some data in DB we'll return
//            if (_context.Users.Any())
//                return;
//            //Ensure that the database exists 
//            _context.Database.EnsureCreated();
//            try
//            {
//                var user = new ApplicationUser { UserName = "Admin", Email = "Admin@hotmail.com" };

//                var result = await _userManager.CreateAsync(user, "Test123$");

//                if (!result.Succeeded)
//                {
//                    throw new Exception("Error");
//                }
//                // We'll read posts data from JSON file located in JSONData folder 
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//    }
//}
