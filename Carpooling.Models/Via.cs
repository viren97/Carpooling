using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.Models {
    public class Via {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public decimal Distance { get; set; }
        public decimal Price { get; set; }
       
    }
}

