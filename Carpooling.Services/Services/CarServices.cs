using Carpooling.DataProvider;
using Carpooling.Models;
using System;

namespace Carpooling.Services {
    public class CarServices : ICarServices {
        private IUserRepository Dbu = new UserRepository();
        private ICarRepository Dbc = new CarRepository();
        public void RegisterCar(User user, int seats, string registrationNumber) {
            try {
                Car car = new Car() {
                    Capacity = seats,
                    RegistrationNumber = registrationNumber
                };

                Dbc.AddCar(car);
                Car carForGettingId = Dbc.GetCarByNoPlate(seats, registrationNumber);
                user.CarId = carForGettingId.Id;
                Dbu.UpdateUser(user);

            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        

        }

        public string RemoveCar(User user) {
            try {
                var car = Dbc.GetCarById(user.CarId);
                user.CarId = -1;
                Dbc.DeleteCar(car);
                Dbu.UpdateUser(user);
                return "Car Removed From User Account...";
            } catch(Exception e) {
                return e.Message;
            }
          
        }
        
        public string BookCarForRide(User user) {
            try {
                var car = Dbc.GetCarById(user.CarId);
                if(car.IsBooked != true) {
                    car.IsBooked = true;
                    Dbu.UpdateUser(user);
                    return "Car is Booked Successfully...";
                }
                else {
                    return "Car is Already Booked...";
                }
            } catch(Exception e) {
                return e.Message;
            }
           
            
        }

    }
}
