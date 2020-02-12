using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.DataProvider {
    public interface IRideRepository {
        void AddRide(Ride ride);
        void DeleteRide(Ride ride);
        void UpdateRide(Ride ride);
        List<Ride> GetRides();
        Ride GetRideById(int id);
        List<Ride> GetRidesByCreatorId(int creatorId);
    }
}
