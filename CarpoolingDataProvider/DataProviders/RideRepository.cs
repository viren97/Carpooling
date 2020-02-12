using Carpooling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpooling.DataProvider {
    public class RideRepository : IRideRepository {

        private CarpoolingContext CarpoolContext = new CarpoolingContext();
        public void AddRide(Ride ride) {
       
                //Ridec ridec = new Ridec() { Source = ride.Source, Destination = ride.Destination, CarId = ride.Car.Id, SeatAvailable = ride.SeatAvailable, UserId = ride.User.Id };
                //CarpoolContext.Ridecs.Add(ridec);
                CarpoolContext.Add(ride);
                CarpoolContext.SaveChanges();
           
        }

        public void DeleteRide(Ride ride) {
         
                CarpoolContext.Rides.Remove(ride);
                CarpoolContext.SaveChanges();

          
        }

        public void UpdateRide(Ride ride) {
         
                CarpoolContext.Entry(ride).State = EntityState.Modified;
                CarpoolContext.SaveChanges();
    
          
        }

        public List<Ride> GetRides() {
        
                return CarpoolContext.Rides.Include(r => r.ViaMaps).ToList();

            
        }

        public Ride GetRideById(int id) {
           
                return CarpoolContext.Rides.Find(id);
            
            
        }

        public List<Ride> GetRidesByCreatorId(int creatorId) {
            return CarpoolContext.Rides.Where(r => r.CreatorId.Equals(creatorId)).Include(r => r.ViaMaps).ToList();
        }

        
    }
}
