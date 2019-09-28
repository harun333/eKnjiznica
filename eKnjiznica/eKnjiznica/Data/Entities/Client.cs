using eKnjiznica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eKnjiznica.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public string applicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
    }
}
