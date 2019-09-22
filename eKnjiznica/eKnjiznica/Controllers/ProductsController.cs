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
    public class SelectViewModel
    {
        public int Id;
        public string DisplayName;
        public bool Selected;
    }

    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<SelectViewModel> Categories { get; set; } = new List<SelectViewModel>();
    }

    //TODO: [Authorize("ADMIN")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository context, ICategoryRepository categoryRepository)
        {
            _repository = context;
            _categoryRepository = categoryRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // TODO: filter products before giving them to the list
            //var queryCollection = Request.Query;
            //foreach (var item in queryCollection)
            //{
            //    //za ovaj url https://docs.microsoft.com/...?view=aspnetcore-2.2&tabs=visual-studio
            //    // prvi krug    
            //    //item.Key -> view
            //    //item.value ->aspnetcore - 2.2
            //    // drugi krug
            //    //item.Key -> tabs
            //    //item.value ->visual-studio

            //    // http://localhost:port/products?asdasdasd
            //}

            var filteredProdcuts = await _repository.FindAllAsync();
            var allCategories = await _categoryRepository.FindAllAsync();

            var vm = new SearchViewModel
            {
                Categories = allCategories,
                Products = filteredProdcuts
            };

            return View(vm);
        }

        

        [HttpPost]
        public async Task<IActionResult> Index(List<SelectViewModel> categories)
        {
            // nadjemo kategorije koje je korisnik izabrao
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

            // nadjemo medju-tabelu koja sadrzi kategorije i produkte na osnovu izabranih
            var filteredProdcutCategoriess = await _repository.FindAllInCategories(filterCategories);
            var filteredProducts = new List<Product>();

            // uzmemo samo produkte
            foreach (var item in filteredProdcutCategoriess)
            {
                filteredProducts.Add(item.Product);
            }

            // napravimo view model
            var vm = new SearchViewModel
            {
                Categories = allCategories,
                Products = filteredProducts
            };

            // ubacimo ih u view
            return View(vm);
        }

        // GET: Products/Details/5
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

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // find the product
            var product = await _repository.FindOne(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            // create a view model to contain all the data we want to display
            var vm = new ProductEditViewModel();

            vm.Product = product;

            // get all categories
            var allCategories = await _categoryRepository.FindAllAsync();
            foreach (var category in allCategories)
            {
                var model = new SelectViewModel();

                model.Id = category.Id;
                model.DisplayName = category.Name;
                model.Selected = false; 

                // put all the categories in the view model since we need to display them
                vm.Categories.Add(model);
            }

            // iterate through all categories in the product
            foreach (var selectedCategory in product.ProductCategories)
            {
                var categorySelection = vm.Categories.FirstOrDefault(m => m.Id == selectedCategory.CategoryId);
                if (categorySelection != null)
                {
                    // mark the view model categories as "selected" if the product has them 
                    categorySelection.Selected = true;
                }
            }

            return View(vm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
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
