using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Carpooling.Models {
    public class Payment {
        public int Id { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("User")]
        public int CreatorId { get; set; }

        [ForeignKey("Rider")]
        public int RiderId { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

    }
}
