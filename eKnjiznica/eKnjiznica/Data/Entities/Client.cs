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
    }
}
