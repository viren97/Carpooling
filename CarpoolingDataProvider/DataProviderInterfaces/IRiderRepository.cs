using Carpooling.Models;
using System.Collections.Generic;

namespace Carpooling.DataProvider {
    public interface IRiderRepository {
        void AddRider(Rider rider);
        void DeleteRider(Rider rider);
        void UpdateRider(Rider rider);
        List<Rider> GetRidersByRideId(int rideId);
        //Rider GetRiderById(int id);
        List<Rider> GetRidersByUserId(int userId);
        List<Rider> GetRiders();
    }
}
