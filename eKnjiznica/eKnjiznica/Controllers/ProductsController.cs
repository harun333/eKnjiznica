using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eKnjiznica.Data.Repositories;
using eKnjiznica.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using eKnjiznica.ViewModels;

namespace eKnjiznica.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(
            IProductRepository context,
            ICategoryRepository categoryRepository)
        {
            _repository = context;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var filteredProdcuts = await _repository.FindAllAsync();
            var allCategories = await _categoryRepository.FindAllAsync();
            var vm = new SearchViewModel
            {
                Categories = allCategories,
                Products = filteredProdcuts,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<SelectViewModel> categories)
        {
            var allCategories = await _categoryRepository.FindAllAsync();
            var filterCategories = new List<Category>();
            foreach (var category in allCategories)
            {
                if (Request.Form.ContainsKey("Category-" + category.Id))
                {
                    if (Request.Form["Category-" + category.Id] == "on")
                    {
                        filterCategories.Add(category);
                    }
                }
            }

            var filteredProdcutCategoriess = await _repository.FindAllInCategories(filterCategories);
            var filteredProducts = new List<Product>();

            foreach (var item in filteredProdcutCategoriess)
            {
                filteredProducts.Add(item.Product);
            }

            var vm = new SearchViewModel
            {
                Categories = allCategories,
                Products = filteredProducts
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.FindOne(id.Value);
             
            if (product == null)
            {
                return NotFound();
            }

            var vm = new ProductDetailsViewModel
            {
                Product = product,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(product);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.FindOne(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var vm = new ProductEditViewModel
            {
                Product = product
            };

            var allCategories = await _categoryRepository.FindAllAsync();
            foreach (var category in allCategories)
            {
                var model = new SelectViewModel
                {
                    Id = category.Id,
                    DisplayName = category.Name,
                    Selected = false
                };

                vm.Categories.Add(model);
            }

            foreach (var selectedCategory in product.ProductCategories)
            {
                var categorySelection = vm.Categories.FirstOrDefault(m => m.Id == selectedCategory.CategoryId);
                if (categorySelection != null)
                {
                    categorySelection.Selected = true;
                }
            }

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product = await _repository.FindOne(id);

                    if (product.ProductCategories.Count() > 0)
                    {
                        _repository.RemoveCategoryRelatiionships(product);
                        await _repository.SaveChangesAsync();
                    }

                    var allCategories = await _categoryRepository.FindAllAsync();
                    var allRelationships = new List<ProductCategory>();
                    foreach (var category in allCategories)
                    {
                        if (Request.Form.ContainsKey("Category-" + category.Id))
                        {
                            if (Request.Form["Category-" + category.Id] == "on")
                            {
                                allRelationships.Add(new ProductCategory
                                {
                                    CategoryId = category.Id,
                                    ProductId = product.Id
                                });
                            }
                        }
                    }

                    _repository.Update(product);
                    _repository.SetCategoryRelationships(allRelationships);

                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.FindOne(id.Value);             
            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _repository.FindOne(id);
            _repository.Remove(product);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _repository.Any(id);
        }
    }
}
