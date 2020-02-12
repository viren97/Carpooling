using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling {
    public class Enum {
        public enum UserInitialOption {
            login = 1,
            Register = 2,
            Exit = 3
        }

        public enum UserOptionHasCar {
            BookARide = 1,
            CancelBookedRide = 2,
            ViewYourBookings = 3,
            ViewAllOffers = 4,
            OfferRide = 5,
            ViewAllBookingsForOffer = 6,
            ViewAllRequests = 7,
            ViewPreviousRides = 8,
            DeleteOfferedRide = 9,
            RegisterCar = 10,
            Logout = 11

        }

        public enum UserOptionNoCar {
            BookARide = 1,
            CancelBookedRide = 2,
            ViewYourBookings = 3,
            ViewAllOffers = 4,
            ViewPreviousRides = 5,
            RegisterACar = 6,
            Logout = 7
        }

    }
}


