using eKnjiznica.Data.Entities;
using System.Collections.Generic;

namespace eKnjiznica.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<SelectViewModel> Categories { get; set; } = new List<SelectViewModel>();
    }
}
