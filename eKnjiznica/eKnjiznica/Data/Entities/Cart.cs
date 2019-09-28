using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }
        public Product Product { get; set; }
    }
}
