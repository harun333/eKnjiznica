using System.Collections.Generic;

namespace eKnjiznica.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string ArtistName { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }

}
