using System.ComponentModel.DataAnnotations.Schema;

namespace Carpooling.Models {
    public class Rider {  
        public int Id { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public decimal RideCost { get; set; }
        public int Seats { get; set; }

        public int UserId { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

        // -1 not a rider, 0 rider but in pending list, 1 booking confirm
        public bool IsBookingConfirm { get; set; }

        public Rider() {
            this.IsBookingConfirm = false;
        }

     

    }
}
