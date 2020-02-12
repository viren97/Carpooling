using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.Models {
    public class Car {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string RegistrationNumber { get; set; }
        public bool IsBooked { get; set; }
    }
}
