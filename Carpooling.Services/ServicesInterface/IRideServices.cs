using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.Services {
    public interface IRideServices {
        string OfferRide(User user, List<Via> via, string source, string destination);
        dynamic ShowAllOfferByYou(User user, ref List<int> rideIds);
        string DeleteOfferedRide(User user, int rideId);
        string ShowAllAvailableOffer();
        Dictionary<int, string> FindRides(string source, string destination);
        decimal CalculateCharge(IList<Via> viaMaps, string source, string destination);
        decimal BookRide(User user, int rideId, string source, string destination, int seat);
        string ViewPendingRequestForRide(User user, int offerId, ref List<int> riderIds);
        string ApproveRideFromPendingList(User user, int offerId, int riderId);
        string ViewAllBookingsForYourOffer(User user, int rideId);
        List<Rider> ShowAllBookingHelper(User user, ref List<int> offerIds);
        string ShowAllBookingByYou(User user, ref List<int> offerIds);
        string CancelBookedRide(User user, int rideId);
        string ViewRideHistory(User user);
        bool IsSeatAvailable(int rideId, int seat);
    }
}
