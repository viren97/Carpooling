using Carpooling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpooling.DataProvider {
    public class CarRepository : ICarRepository {
        private CarpoolingContext CarpoolContext = new CarpoolingContext();
        public void AddCar(Car car) {
            CarpoolContext.Cars.Add(car);
            CarpoolContext.SaveChanges();
        }

        public void DeleteCar(Car car) {
            CarpoolContext.Cars.Remove(car);
            CarpoolContext.SaveChanges();
        }

        public void UpdateCar(Car car) {
            CarpoolContext.Entry(car).State = EntityState.Modified;
            CarpoolContext.SaveChanges();
        }

        public List<Car> GetCars() {
            return CarpoolContext.Cars.ToList();
        }

        public Car GetCarById(int id) {
            return CarpoolContext.Cars.Find(id);
        }

        public Car GetCarByNoPlate(int seats, string registrationNumber) {
            return CarpoolContext.Cars.FirstOrDefault(c => c.Capacity.Equals(seats) && c.RegistrationNumber.Equals(registrationNumber));
        }
    }
}
