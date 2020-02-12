using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.DataProvider {
    public interface ICarRepository {
        void AddCar(Car car);
        void DeleteCar(Car car);
        void UpdateCar(Car car);
        List<Car> GetCars();
        Car GetCarById(int id);
        Car GetCarByNoPlate(int seats, string registrationNumber);

    }
}
