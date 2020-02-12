using Carpooling.DataProvider;
using Carpooling.DataProvider.DataProviderInterfaces;
using Carpooling.DataProvider.DataProviders;
using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpooling.Services {
    public class RideServices : IRideServices {
        private IRideRepository Dbr = new RideRepository();
        private IRiderRepository Dbrr = new RiderRepository();
        private ICarRepository Dbc = new CarRepository();
        private IPaymentRepository Dbp = new PaymentRepository();


        public string OfferRide(User user, List<Via> via, string source, string destination) {
            Ride ride = new Ride() {
                Source = source,
                Destination = destination,
                CreatorId = user.Id,
                CarId = user.CarId,
              
                ViaMaps = via
            };

            Car car = Dbc.GetCarById(user.CarId);
            ride.SeatAvailable = car.Capacity - 1;

            try {
                Dbr.AddRide(ride);
                //Dbrr.AddRider(rider);
                return String.Format("Offered Registered Successfully...\n Ride Information\tSource : {0,-11}  Destination : {1,-17}  Ride Id : {2,-20}", ride.Source, ride.Destination, ride.Id);
            }
            catch (Exception e) {
                return "Ride Registration Unsuccessful..  " + e.Message;

            }
        }

        public dynamic ShowAllOfferByYou(User user, ref List<int> rideIds) {
            try {
                var ridesByUser = Dbr.GetRidesByCreatorId(user.Id);
                //var ridesByUser = rides.Where(r => r.CreatorId.Equals(user.Id)).ToList();
                string rideHeader = string.Format("{0,-10}{1,-11}{2,-16}{3,-20}", "Index", "Source", "Destination", "Offer ID");
                int i = 1;

                string rideInformation = string.Empty;
                foreach (Ride ride in ridesByUser) {
                    rideInformation = string.Concat(rideInformation, "\n", string.Format("{0,-10}{1,-11}{2,-16}{3,-20}", i++, ride.Source, ride.Destination, ride.Id));
                    rideIds.Add(ride.Id);
                }

                string rideInfo = string.Concat(rideHeader, "\n", rideInformation);

                return rideInfo;
            }
            catch (Exception) {
                return "Can't Fatch Offers for Now... Try later!";
            }

        }

        public string DeleteOfferedRide(User user, int rideId) {
            Ride ride = Dbr.GetRideById(rideId);
            Dbr.DeleteRide(ride);
            return string.Format("Offer Source : {0}, Destination : {1}, ID : {2} is Removed...", ride.Source, ride.Destination, ride.Id);
        }

        public string ShowAllAvailableOffer() {
            List<Ride> rides = Dbr.GetRides();
            if (!rides.Any()) return "NO RIDES AVAILABLE...";
            StringBuilder offerDetails = new StringBuilder();

            offerDetails.Append(string.Format("{0,-10}{1, -11 }{2,-16}{3,-20}", "Index", "Source", "Destination", "SeatAvailable"));
            //Bug is Here in the format
            int i = 1;
            foreach (Ride ride in rides) {
                offerDetails.Append(string.Format("\n{0,-10}{1, -11 }{2,-16}{3,-20}", i++, ride.Source, ride.Destination, ride.SeatAvailable));
                offerDetails.Append("\n[Via Places]\n" + string.Format("{0,-11}{1,-16}{2,-13}{3,-10}","Source", "Destination", "Distance", "Price"));
                foreach (Via via in ride.ViaMaps) {
                    offerDetails.Append(string.Format("\n{0,-11}{1,-16}{2,-13}{3,-10}", via.Source, via.Destination, via.Distance, via.Price));
                }
                i++;

            }
            return offerDetails.ToString();
        }

        public Dictionary<int, string> FindRides(string source, string destination) {
            Dictionary<int, string> suitableRides = new Dictionary<int, string>();
            string viaPoints = string.Empty, rideInfo = string.Empty;
            int f;

            var rides = Dbr.GetRides();
            foreach (var ride in rides) {
                f = 0;
                viaPoints = String.Empty;
                foreach (var via in ride.ViaMaps) {
                    if (via.Source.Equals(source)) {
                        f += 1;
                    }
                    if (via.Destination.Equals(destination)) {
                        f += 1;
                    }
                    if (f == 2) {
                        viaPoints = String.Format("{0,-11}{1,-16}{2,-13}{3,-10}", "Source", "Destination", "Distance", "Price");
                        foreach (var t in ride.ViaMaps) {
                            viaPoints += ("\n" + String.Format("{0,-11}{1,-16}{2,-13}{3,-10}", t.Source, t.Destination, t.Distance, t.Price));
                        }
                        rideInfo = String.Format("{0,-11}{1,-17}{2,-9}{3,-20}\n VIA PLACES \n{4}", source, destination, CalculateCharge(ride.ViaMaps, source, destination), ride.SeatAvailable, viaPoints);
                        suitableRides.Add(ride.Id, rideInfo);
                        break;
                    }
                }
            }
            return suitableRides;

        }

        public decimal CalculateCharge(IList<Via> viaMaps, string source, string destination) {
            if (source.Equals(destination)) {
                return -1;
            }
            decimal cost = 0;
            int flag = -1;
            foreach (var via in viaMaps) {
                if (via.Source.Equals(source)) {
                    cost += via.Price;
                    flag = 1;
                    continue;
                }
                if (via.Destination.Equals(destination)) {
                    cost += via.Price;
                    break;

                }

                if (flag == 1) {
                    cost += via.Price;
                }
            }
            return cost;
        }

        public decimal BookRide(User user, int rideId, string source, string destination, int seat) {
            decimal cost = 0;
            var ride = Dbr.GetRideById(rideId);
            if (ride != null) {
                cost = CalculateCharge(ride.ViaMaps, source, destination);
                var rider = new Rider() {
                    UserId = user.Id,
                    Source = source,
                    Destination = destination,
                    RideCost = cost * seat,
                    Seats = seat,
                    RideId = ride.Id
                };

                //ride.Riders.Add(rider.Id);
                var payment = new Payment() {
                    CreatorId = ride.CreatorId,
                    RiderId = user.Id,
                    Price = cost * seat,
                    RideId = ride.Id
                };
                try {
                    Dbrr.AddRider(rider);
                    Dbp.AddPayment(payment);
                    Dbr.UpdateRide(ride);
                    return cost * seat;
                }catch(Exception) {
                    return -1;
                }
           
            }
            return -1;
        }

        public string ViewPendingRequestForRide(User user, int offerId, ref List<int> riderIds) {
            try {
                Ride ride = Dbr.GetRideById(offerId);
                if (!ride.CreatorId.Equals(user.Id)) return "Offer is not belong to you...";
                StringBuilder bookingInfo = new StringBuilder("");
                bookingInfo.Append(string.Format("{0,-10}{1,-11}{2,-16}{3,-9}{4,-10}", "Index", "Source", "Destination", "seats", "Rider Id"));
                int i = 1;
                //ride.Riders.Where(r => r.IsBookingConfirm == false).ToList()
                foreach (Rider rider in Dbrr.GetRidersByRideId(ride.Id).Where(r => r.IsBookingConfirm == false)) {
                    bookingInfo.Append(string.Format("\n{0,-10}{1,-11}{2,-16}{3,-9}{4,-10}", i++, rider.Source, rider.Destination, rider.Seats, ride.Id));
                    riderIds.Add(rider.Id);
                }
                return i == 1 ? "No Booking is Here for your Ride..." : bookingInfo.ToString();

            }
            catch (Exception) {
                return "Ride is not Available...";
            }
        }

        public string ApproveRideFromPendingList(User user, int offerId, int riderId) {

            try {
                var ride = Dbr.GetRideById(offerId);
                if (ride == null) return "Error : No Ride is Available...";
                if (!ride.CreatorId.Equals(user.Id)) return "Ride does not belong to you...";

                var Riders = Dbrr.GetRidersByRideId(ride.Id);

                var rider = Riders.FirstOrDefault(r => r.Id.Equals(riderId) && r.IsBookingConfirm == false);
                if (rider != null && ride != null) {
                    if (ride.SeatAvailable >= rider.Seats) {
                        ride.SeatAvailable -= rider.Seats;
                        rider.IsBookingConfirm = true;
                        Dbr.UpdateRide(ride);

                        return string.Format("Booking Confirmed.. Source {0} to Destination {1}", rider.Source, rider.Destination);
                    }
                    else {
                        return string.Format("Seats is not Availble..  Seat Left {0}", ride.SeatAvailable);
                    }
                }
                else {
                    return string.Format("Ride or Rider in pending list not Available...");
                }

            }
            catch (Exception) {
                return "Select Valid Ride or Rider";
            }

        }

        public string ViewAllBookingsForYourOffer(User user, int rideId) {

            try {
                var ride = Dbr.GetRideById(rideId);

                int i = 1;
                string bookingInfo = string.Empty;
                var Riders = Dbrr.GetRidersByRideId(ride.Id);
                foreach (Rider rider in Riders) {
                    bookingInfo = string.Concat(bookingInfo, "\n", string.Format("{0,-10}{1,-11}{2,-17}{3,-20}", i++, rider.Source, rider.Destination, rider.Id));


                }
                return bookingInfo.Length == 0 ? "No bookings available" : bookingInfo;
            }
            catch (Exception) {
                return "Error... No booking available or No offer is offered by you...";
            }

        }

        public List<Rider> ShowAllBookingHelper(User user, ref List<int> offerIds) {
            var riders = Dbrr.GetRidersByUserId(user.Id);
            foreach(Rider rider in riders) {
                offerIds.Add(rider.RideId);
            }
            return riders;

            //var Rides = Dbr.GetRides();
            //var filteredRides = Rides.Where(r => r.Riders.FirstOrDefault(p => p.Id.Equals(user.Id)) != null);
            //foreach (Ride ride in filteredRides) {
            //    Rider rider = ride.Riders.FirstOrDefault(p => p.Id.Equals(user.Id));
            //    offerIds.Add(ride.Id);
            //    riders.Add(rider);
            //}
        }

        public string ShowAllBookingByYou(User user, ref List<int> offerIds) {
            try {
                string bookingInfo = string.Empty;
                int i = 1;
                foreach (Rider rider in ShowAllBookingHelper(user, ref offerIds)) {

                    bookingInfo = string.Concat(bookingInfo, "\n", string.Format("{0,-10}{1,-11}{2,-17}{3,-9}", i++, rider.Source, rider.Destination, rider.RideCost));
                }

                return bookingInfo.Length == 0 ? "No bookings Available..." : bookingInfo;
            }
            catch (Exception) {
                return "No bookings Available...";
            }
        }

        public string CancelBookedRide(User user, int rideId) {
            try {
                var Riders = Dbrr.GetRidersByRideId(rideId);
                var rider = Riders.FirstOrDefault(p => p.Id.Equals(user.Id));
                if (rider.IsBookingConfirm) {
                    var ride = Dbr.GetRideById(rideId);
                    ride.SeatAvailable += 1;
                    Dbr.UpdateRide(ride);
                }
                Dbrr.DeleteRider(rider);
                return "Booking Cancelled Successfully...";
            }
            catch (Exception) {
                return "Booking Can't be Cancelled..";
            }

        }

        public string ViewRideHistory(User user) {
            string rideHistory = string.Empty;
            var riders = Dbrr.GetRiders();
            var ridesCreatedByUser = Dbr.GetRidesByCreatorId(user.Id);

            int i = 1;
            foreach (Ride ride in ridesCreatedByUser) {
                rideHistory = string.Concat(rideHistory, "\n", string.Format("{0,-10}{1,-11}{2,-16}{3,-12}", i++, ride.Source, ride.Destination, "Creator"));
            }

            foreach(Rider rider in riders.Where(r => r.IsBookingConfirm)) {
                rideHistory = string.Concat(rideHistory, "\n", string.Format("{0,-10}{1,-11}{2,-16}{3,-12}", i++, rider.Source, rider.Destination, rider.RideCost));
            }

            return rideHistory.Length == 0 ? "No Previous Ride Available" : rideHistory;

        }

        public bool IsSeatAvailable(int rideId, int seat) {
            var ride = Dbr.GetRideById(rideId);

            if (ride.SeatAvailable >= seat) {
                return true;
            }
            return false;

        }


    }
}
