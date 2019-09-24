using System;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!_context.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "A"
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = "2",
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = "C"
                });
            }

            var mainAdminUser = await _userManager.FindByEmailAsync("Admin@hotmail.com");
            if (mainAdminUser != null)
                return View();

            _context.Database.EnsureCreated();

            try
            {
                var user = new ApplicationUser
                {
                    Email = "Admin@hotmail.com",
                    NormalizedEmail = "ADMIN@HOTMAIL.COM",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                var result = await _userManager.CreateAsync(user, "Test2!");
                await _context.SaveChangesAsync();

                var savedUser = await _userManager.FindByEmailAsync("Admin@hotmail.com");

                if (!result.Succeeded)
                {
                    throw new Exception("Error");                   
                }

                await _userManager.AddToRoleAsync(user, "Admin");

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> DeleteMainAdmin()
        {
            var mainAdminUser = await _userManager.FindByEmailAsync("Admin@hotmail.com");
            await _userManager.RemoveFromRoleAsync(mainAdminUser, "Admin");
            await _userManager.DeleteAsync(mainAdminUser);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
