using Carpooling.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace Carpooling.DataProvider {
    public class RiderRepository : IRiderRepository {
        private CarpoolingContext CarpoolContext = new CarpoolingContext();
        public void AddRider(Rider rider) {
            CarpoolContext.Riders.Add(rider);
            CarpoolContext.SaveChanges();
        }

        public void DeleteRider(Rider rider) {
            CarpoolContext.Riders.Remove(rider);
            CarpoolContext.SaveChanges();
        }

        public void UpdateRider(Rider rider) {
            CarpoolContext.Entry(rider).State = EntityState.Modified;
            CarpoolContext.SaveChanges();
        }

        public List<Rider> GetRidersByRideId(int rideId) {
            return CarpoolContext.Riders.Where(r => r.RideId.Equals(rideId)).ToList();
        }

        public List<Rider> GetRidersByUserId(int userId) {
            return CarpoolContext.Riders.Where(r => r.UserId.Equals(userId)).ToList();
        }

        public List<Rider> GetRiders() {
            return CarpoolContext.Riders.ToList();
        }


        //CANT BE USED, IT CAUSE ISSUE WHILE FETCHING RESULT. THERE CAN BE MULTIPLE RIDER 
        //public Rider GetRiderById(int id) {
        //    return CarpoolContext.Riders.Find(id);
        //}
    }
}
