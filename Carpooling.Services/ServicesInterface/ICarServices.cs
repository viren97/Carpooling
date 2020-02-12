using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.Services {
    public interface ICarServices {
        void RegisterCar(User user, int seats, string registrationNumber);
        string RemoveCar(User user);
        string BookCarForRide(User user);
    }
}
