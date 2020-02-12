using Carpooling.DataProvider;
using Carpooling.DataProvider.DataProviders;
using Carpooling.Models;
using Carpooling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CarpoolingEF {
    class Program {
        private IRideServices RideServices = new RideServices();
        private IUserServices UserServices = new UserServices();
        private ICarServices CarServices = new CarServices();

        private User LoggedUser { get; set; }

        public void UserOptionsNoCar() {
            Console.WriteLine("1 : Book a Ride");
            Console.WriteLine("2 : Cancel Booked Ride");
            Console.WriteLine("3 : View Your Bookings ");
            Console.WriteLine("4 : View All Offers");
            Console.WriteLine("5 : View Previous Rides");
            Console.WriteLine("6 : Register Your Car");
            Console.WriteLine("7 : Logout");
        }

        public void UserOptionsHasCar() {
            Console.WriteLine("1 : Book a Ride");
            Console.WriteLine("2 : Cancel Booked Ride");
            Console.WriteLine("3 : View Your Bookings ");
            Console.WriteLine("4 : View All Offers");
            Console.WriteLine("5 : Offer a Ride");
            Console.WriteLine("6 : View All Bookings for your offer");
            Console.WriteLine("7 : View All Booking Request for Your Offer");
            Console.WriteLine("8 : View Previous Rides");
            Console.WriteLine("9 : Delete Offered Ride");
            Console.WriteLine("10 : Register Your Car");
            Console.WriteLine("11 : Logout");
        }

        public bool OfferARide() {
            try {
                List<Via> viaplaces = new List<Via>();
                List<string> via = new List<string>();
                string source, destination, countPlaces, tempPlace;


                Console.WriteLine("Enter Source of Journey / Enter to Exit ..");
                source = Console.ReadLine();
                if (source.Equals(""))
                    return true;

                Console.WriteLine("Enter Destination of Journey / Enter to Exit ..");
                destination = Console.ReadLine();
                if (destination.Equals("")) return true;

                Console.WriteLine("Enter Number of places you are covering including source and destination / Enter to Exit ..");
                countPlaces = (Console.ReadLine());


                do {
                    Console.WriteLine("Enter Via Places including Source and Destination in sequence / Enter to Exit ..");
                    for (int j = 0; j < Int32.Parse(countPlaces); j++) {
                        tempPlace = Console.ReadLine();
                        if (tempPlace.Equals("")) return true;
                        via.Add(tempPlace);
                    }

                    if (!source.Equals(via[0]) || !destination.Equals(via[via.Count - 1])) {
                        Console.WriteLine("Source and Destination are different from initial inputs. Please Enter Valid data...\n");
                        via.Clear();
                        continue;
                    }
                    break;
                } while (true);



                decimal d, c;
                string temp;
                Via viaobj;
                for (int j = 1; j < via.Count; j++) {
                    do {
                        Console.WriteLine("Enter Distance Between {0} and {1} / Enter -1 to Exit..", via[j - 1], via[j]);
                        temp = Console.ReadLine();
                        if (!decimal.TryParse(temp, out d)) { Console.WriteLine("Enter Decimal Digits! strings are not valid."); continue; }
                        if (d == -1) return true;
                        break;
                    } while (true);


                    do {
                        Console.WriteLine("Enter Travel cost Between {0} and {1}/Enter -1 to Exit..", via[j - 1], via[j]);
                        temp = Console.ReadLine();
                        if (!decimal.TryParse(temp, out c)) { Console.WriteLine("Enter Decimal Digits! strings are not valid."); continue; }
                        if (c == -1) return true;
                        break;
                    } while (true);

                    viaobj = new Via() { Source = via[j - 1], Destination = via[j], Distance = d, Price = c };
                    viaplaces.Add(viaobj);


                }

                string rideInfo = RideServices.OfferRide(LoggedUser, viaplaces, source, destination);
                Console.WriteLine(rideInfo);


            }
            catch (Exception ex) {
                Console.WriteLine("Error... Enter Valid data"+ex.InnerException);
            }
            return true;
        }

        public bool DeleteOfferedRide() {
            try {
                List<int> offerIds = new List<int>();

                var offerInfos = RideServices.ShowAllOfferByYou(LoggedUser, ref offerIds);
                if (offerIds.Count == 0) {
                    Console.WriteLine("No offer to Delete..");
                    return true;
                }
                Console.WriteLine(offerInfos);
                do {
                    Console.WriteLine("Select an Option from above...");
                    try {
                        int opt = Int32.Parse(Console.ReadLine());
                        RideServices.DeleteOfferedRide(LoggedUser, offerIds[opt - 1]);
                        Console.WriteLine("Offer Removed Successfully...");
                        return true;
                    }
                    catch (Exception) {
                        Console.WriteLine("Input Valid Option...");
                        continue;
                    }

                } while (true);


            }
            catch (Exception) {
                Console.WriteLine("Error...No offer avialable");
            }
            return true;

        }

        public bool ViewAllOffers() {
            try {
                int i = 1;
                Console.WriteLine(RideServices.ShowAllAvailableOffer());
            }
            catch (Exception) {
                Console.WriteLine("No offers for now, Check later or create one!");
            }

            return true;
        }

        public bool BookARide() {
            string source, destination;
            Console.WriteLine("Enter Source / Enter to Exit ..");
            source = Console.ReadLine();
            if (source.Equals("")) return true;
            Console.WriteLine("Enter Destination / Enter to Exit ..");
            destination = Console.ReadLine();
            if (destination.Equals("")) return true;
            try {
                int i = 1;
                List<int> rideIds = new List<int>();
                Dictionary<int, string> rideInfos = RideServices.FindRides(source, destination);
                if (rideInfos.Count == 0) {
                    Console.WriteLine("No Ride Available from {0}  to  {1}", source, destination);
                    return true;
                }
                Console.WriteLine("{0,-10}{1,-11}{2,-17}{3,-9}{4,-20}", "Index", "Source", "Destination", "Cost", "Seat Available");
                foreach (var rideinfo in rideInfos) {
                    Console.WriteLine("{0,-10}{1}", i++, rideinfo.Value);
                    Console.WriteLine();
                    Console.WriteLine();
                    rideIds.Add(rideinfo.Key);
                }



                Console.WriteLine("Select One Ride from Above...");
                int opt = Int32.Parse(Console.ReadLine());
                int seat;
                do {
                    Console.WriteLine("Enter the Seat Required...");
                    seat = Int32.Parse(Console.ReadLine());
                    if (!RideServices.IsSeatAvailable(rideIds[opt - 1], seat)) {
                        Console.WriteLine("Required Seats Count is not Available...\nIf you want to book ride, you can do it by reducing the seat count to availability.. Enter y/n");
                        if (Console.ReadLine().Equals("y")) {
                            continue;
                        }
                        return true;

                    }
                    break;
                } while (true);


                decimal cost = RideServices.BookRide(LoggedUser, rideIds[opt - 1], source, destination, seat);
                if (cost == -1) {
                    Console.WriteLine("Rider is already registered...");
                    return true;
                }
                Console.WriteLine("Ride in on Pending List, Amount to be Paid is : " + cost);




            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error: No Ride Available for this Source and Destination...");
            }


            return true;
        }

        public bool CancelBookedRide() {
            List<int> offerIds = new List<int>();
            string bookingInfos = RideServices.ShowAllBookingByYou(LoggedUser, ref offerIds);
            if (offerIds.Count == 0) { Console.WriteLine("No bookings Available to Cancel..."); return true; }
            Console.WriteLine("{0,-10}{1,-11}{2,-17}{3,-9}", "Index", "Source", "Destination", "Ride Cost");
            Console.WriteLine(bookingInfos);
         

            do {
                Console.WriteLine("Select an Option from above...");
                try {
                    int opt = Int32.Parse(Console.ReadLine());
                    Console.WriteLine(RideServices.CancelBookedRide(LoggedUser, offerIds[opt - 1]));
                }
                catch (Exception) {
                    Console.WriteLine("Input Valid Option...");
                    continue;
                }
                return true;
            } while (true);
        }

        public bool ViewAllBookings() {
            Console.WriteLine("\nIncluding You...");
            try {
                List<int> offerIds = new List<int>();
                Console.WriteLine(RideServices.ShowAllOfferByYou(LoggedUser, ref offerIds));
                if (offerIds.Count == 0) {
                    Console.WriteLine("You did not offer any Ride...");
                    return true;
                }
                Console.WriteLine("Select any Offer to See Their bookings...");
                int opt = Console.Read();
                Console.WriteLine("{0,-10}{1,-11}{2,-17}{3,-20}", "Index", "Source", "Destination", "Rider Id");

                Console.WriteLine(RideServices.ViewAllBookingsForYourOffer(LoggedUser, offerIds[opt - 1]));
            }
            catch (Exception) {
                Console.WriteLine("No bookings available, or You are not offered any Ride...");
            }

            return true;
        }

        public bool ViewPreviousRide() {
            try {

                Console.WriteLine("{0,-10}{1,-11}{2,-17}{3,-9}", "Index", "Source", "Destination", "Cost");
                Console.WriteLine(RideServices.ViewRideHistory(LoggedUser));

            }
            catch (Exception) {
                Console.WriteLine("No previous Record...");
            }

            return true;
        }

        public bool ViewBookingRequests() {
            try {
                List<int> riderIds = new List<int>();
                List<int> offerIds = new List<int>();

                string allOffers = RideServices.ShowAllOfferByYou(LoggedUser, ref offerIds);
                if (!offerIds.Any()) {
                    Console.WriteLine("No Ride you have offered...");
                    return true;
                }
                Console.WriteLine(allOffers);
                Console.WriteLine("\nSelect one Offer to See any Pending Request...");
                int option = Console.Read();
                string pendingBookings = RideServices.ViewPendingRequestForRide(LoggedUser, offerIds[option - 1], ref riderIds);

                if (!riderIds.Any()) {
                    Console.WriteLine("No Pending Requests");
                    return true;
                }
                Console.WriteLine(pendingBookings);

                int i = riderIds.Count;
                int opt;
                string message;
                while (i-- > 0) {
                    Console.WriteLine("Select Rider or press 0000 to exit");
                    opt = Int32.Parse(Console.ReadLine());
                    if (opt == 0000) {
                        break;
                    }
                    message = RideServices.ApproveRideFromPendingList(LoggedUser, offerIds[option - 1], riderIds[opt - 1]);
                    if (string.IsNullOrEmpty(message))
                        break;
                    Console.WriteLine(message);
                }
            }
            catch (Exception) {
                Console.WriteLine("No Pending Request for your Ride...");
            }

            return true;
        }

        public bool RegisterCar() {
            do {
                try {
                    string registerNumber, seats;

                    Console.WriteLine("Enter Registration Number...  / Enter to Exit ..");
                    registerNumber = Console.ReadLine();
                    if (registerNumber.Equals("")) return true;
                    Console.WriteLine("Enter Number of seats / Enter to Exit ..");
                    seats = (Console.ReadLine());
                    if (seats.Equals("")) return true;
                    CarServices.RegisterCar(LoggedUser, Int32.Parse(seats), registerNumber);
                    Console.WriteLine("Car is registered successfully...");

                    return true;
                }
                catch (Exception) {
                    Console.WriteLine("InValid Input... Do you want to continue Regitering a Car?? y/n");
                    string y = Console.ReadLine();
                    if (y.Equals("n")) {
                        return true;
                    }

                    continue;
                }
            } while (true);
        }

        public bool ViewYourBookings() {
            List<int> offerIds = new List<int>();
            string Rideinfos = RideServices.ShowAllBookingByYou(LoggedUser, ref offerIds);
            if (offerIds.Count == 0) {
                Console.WriteLine("No Previous Bookings...");
                return true;
            }

            Console.WriteLine("{0,-10}{1,-11}{2,-17}{3,-9}", "Index", "Source", "Destination", "Cost");
            Console.WriteLine(Rideinfos);

            return true;
        }

        public bool UserAction() {
            if (LoggedUser.CarId == -1) {
                UserOptionsNoCar();
                int opt = RangeInput(1, 7);
                if (opt == (int)Carpooling.Enum.UserOptionNoCar.BookARide) {
                    return BookARide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionNoCar.CancelBookedRide) {
                    return CancelBookedRide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionNoCar.ViewYourBookings) {
                    return ViewYourBookings();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionNoCar.ViewAllOffers) {
                    return ViewAllOffers();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionNoCar.ViewPreviousRides) {
                    return ViewPreviousRide();
                }
                else if (opt == (int)Carpooling.Enum.UserOptionNoCar.RegisterACar) {
                    return RegisterCar();
                }
                else {
                    LoggedUser = null;
                    return false;
                }

            }
            else {
                UserOptionsHasCar();
                int opt = RangeInput(1, 11);
                if (opt == (int)Carpooling.Enum.UserOptionHasCar.BookARide) {
                    return BookARide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.CancelBookedRide) {
                    return CancelBookedRide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.ViewYourBookings) {
                    return ViewYourBookings();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.ViewAllOffers) {
                    return ViewAllOffers();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.OfferRide) {
                    return OfferARide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.ViewAllBookingsForOffer) {
                    return ViewAllBookings();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.ViewAllRequests) {
                    return ViewBookingRequests();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.ViewPreviousRides) {
                    return ViewPreviousRide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.DeleteOfferedRide) {
                    return DeleteOfferedRide();

                }
                else if (opt == (int)Carpooling.Enum.UserOptionHasCar.RegisterCar) {
                    return RegisterCar();
                }
                else {
                    LoggedUser = null;
                    return false;
                }

            }
}

        #region[InputPassword]
        public void InputPassword(out string password) {
            password = string.Empty;
            do {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0) {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                else if (key.Key == ConsoleKey.Enter) {
                    break;
                }
            } while (true);
            Console.WriteLine();
        }
        #endregion

        public User Authentication() {
            string username, password;
            Console.WriteLine("Enter Username ");
            username = Console.ReadLine();
            Console.WriteLine("Enter Password ");
            InputPassword(out password);
            return UserServices.Login(username, password);
        }


        public void InitialUserOptions() {
            Console.WriteLine("WELCOME TO CARPOOLING APPLICATION ");
            Console.WriteLine("1 : Login ");
            Console.WriteLine("2 : Register Yourself ");
            Console.WriteLine("3 : Exit ");
        }

        public bool UserAuthentication() {
            InitialUserOptions();
            int option = RangeInput(1, 3);
            if (option == (int)Carpooling.Enum.UserInitialOption.login) {
                LoggedUser = Authentication();
                if (LoggedUser != null) {
                    return true;
                }
                else {
                    Console.WriteLine("Invalid User Credential...\n");
                    Console.WriteLine("Register yourself? y/n");
                    string a = Console.ReadLine();

                    if (a.Equals("y")) {

                        LoggedUser = Registration();
                        if (LoggedUser == null) {
                            Console.WriteLine("Registration Unsuccessful...");
                            return false;
                        }
                        return true;
                    }
                    return false;
                }
            }

            else if (option == (int)Carpooling.Enum.UserInitialOption.Register) {
                LoggedUser = Registration();
                if (LoggedUser == null) {
                    Console.WriteLine("Registration Unsuccessful...");
                    return false;
                }
                return true;
            }
            else if (option == (int)Carpooling.Enum.UserInitialOption.Exit) {
                Environment.Exit(0);
            }
            return false;

        }

        public int RangeInput(int a, int b) {
            Console.WriteLine("Select One Option From above Menu...");
            try {
                int c = Int32.Parse(Console.ReadLine());
                if (c <= b && c >= a) {
                    return c;
                }
            }
            catch (Exception) {
                Console.WriteLine("Invalid Option...\nEnter Valid given Option!");

            }
            return RangeInput(a, b);

        }

        public bool ValidateContactNumber(string contact) {
            string s = @"^[0-9]{10}$";
            Regex contactregex = new Regex(s);
            return contactregex.IsMatch(contact);
        }


        public User Registration() {
            string name, contact, address;
            string repassword, username, password;
            Console.WriteLine("Enter Name / Enter to Exit .. ");
            name = Console.ReadLine();
            if (name.Equals("")) return null;
            do {
                Console.WriteLine("Enter contact Number / Enter to Exit .. ");
                contact = Console.ReadLine();
                if (contact.Equals("")) return null;
                if (!ValidateContactNumber(contact)) {
                    Console.WriteLine("Not Valid Phone Number. Try Again...");

                    continue;
                }
                break;
            } while (true);

            Console.WriteLine("\n[Follow] Username and Password Should be Atleast 6 character");
            do {
                Console.WriteLine("Set a Username for Login / Enter to Exit .. ");
                username = Console.ReadLine();
                if (username.Equals("")) return null;
                if (username.Length < 6) { Console.WriteLine("Username should atleast 6 character"); continue; }
                if (!UserServices.IsUsernameAlreadyExist(username)) { Console.Write("Username already Exist... Try with other username!"); continue; }
                break;
            } while (true);



            do {
                Console.WriteLine("Set a Password for Login / Enter to Exit .. ");
                InputPassword(out password);
                if (password.Equals("")) return null;
                if (password.Length < 6) { Console.WriteLine("Password should atleast 6 character"); continue; }
                Console.WriteLine("Re-enter Password for Login / Enter to Exit .. ");
                InputPassword(out repassword);
                if (repassword.Equals("")) return null;
                if (!password.Equals(repassword)) {
                    Console.WriteLine("Both Password are not same...\nTry Again");

                    continue;

                }
                break;
            } while (true);

            Console.WriteLine("Enter Your Address : ");
            address = Console.ReadLine();

            return UserServices.Register(name, contact, address, username, password);
        }



        static void Main(string[] args) {
            Program driver = new Program();
            //StationRepository st = new StationRepository();
            //CarRepository carr = new CarRepository();
            //RideRepository r = new RideRepository();

            ////var car = carr.GetCarById(2);
            ////var ride = r.GetRideById(1);
            //var station = new Ridec() { Source = "a", Destination = "b", CarId = 4, UserId = 5, SeatAvailable = 5};
            //st.AddStation(station);
            //Console.WriteLine(st.GetStation(1).Source);
            //Console.ReadKey();

            bool loginflag = false;
            string exitOption;
            while (true) {
                if (loginflag) {
                    loginflag = driver.UserAction();
                    Console.WriteLine("Do you want to continue with system? y/n");
                    exitOption = Console.ReadLine();

                    if (exitOption.Equals("n"))
                        Environment.Exit(0);
                }
                else {
                    loginflag = driver.UserAuthentication();

                }

            }

        }
    }
}
