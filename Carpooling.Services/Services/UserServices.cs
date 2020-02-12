using Carpooling.DataProvider;
using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpooling.Services {
    public class UserServices : IUserServices {
        private IUserRepository Dbu = new UserRepository();
        public User Login(string username, string password) {
            var users = Dbu.GetUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));
            if (user != null) {
                return user;
            }
            else {
                return null;
            }
        }

        public bool IsUsernameAlreadyExist(string username) {
            var users = Dbu.GetUsers();

            var alreadyExistUser = users.Where(u => u.Username.Equals(username)).FirstOrDefault();
            if (alreadyExistUser != null) {
                return false ;
            }
            return true;
        }

        public User Register(string name, string contact, string address, string username, string password) {


            User user = new User() {
                Name = name,
                Contact = contact,
                Address = address,
                Username = username,
                Password = password,
                CarId = -1
            };


            Dbu.AddUser(user);
            return user;

        }
    }
}
