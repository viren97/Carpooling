using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpooling.Models {
    public class Ride {
        public int Id { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }
        public int SeatAvailable { get; set; }

        [ForeignKey("User")]
        public int CreatorId { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        //public List<int> Riders { get; set; }

        //public List<int> Payments { get; set; }

        public List<Via> ViaMaps { get; set; }

        public Ride() {
            //Riders = new List<int>();
            //Payments = new List<int>();
            ViaMaps = new List<Via>();

        }
    }
}
