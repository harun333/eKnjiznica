using eKnjiznica.Data.Entities;
using System.Collections.Generic;

namespace eKnjiznica.ViewModels
{
    public class ProductEditViewModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        ////public string ArtistName { get; set; }
        //public float Price { get; set; }
        //public string Description { get; set; }


        public List<SelectViewModel> Categories { get; set; } = new List<SelectViewModel>();
        public Product Product { get; internal set; }
    }
}
