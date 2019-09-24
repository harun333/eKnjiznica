using eKnjiznica.Data.Entities;
using System.Collections.Generic;

namespace eKnjiznica.ViewModels
{
    public class SearchViewModel
    {
        public List<Category> Categories;
        public List<Product> Products;
        public bool IsAdmin;
    }
}
