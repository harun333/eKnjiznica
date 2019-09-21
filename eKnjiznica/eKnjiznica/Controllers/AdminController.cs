using eKnjiznica.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eKnjiznica.Controllers
{
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
