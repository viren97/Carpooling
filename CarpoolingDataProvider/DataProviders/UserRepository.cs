using Carpooling.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Carpooling.DataProvider {
    public class UserRepository : IUserRepository {
        private CarpoolingContext CarpoolContext = new CarpoolingContext();

        public void AddUser(User user) {
            CarpoolContext.Users.Add(user);
            CarpoolContext.SaveChanges();
        }

        public void DeleteUser(User user) {
            CarpoolContext.Users.Remove(user);
            CarpoolContext.SaveChanges();
        }

        public void UpdateUser(User user) {
            CarpoolContext.Entry(user).State = EntityState.Modified;
            CarpoolContext.SaveChanges();
        }

        public List<User> GetUsers() {
            return CarpoolContext.Users.ToList();
        }

        public User GetUserById(int id) {
            return CarpoolContext.Users.Find(id);
        }
    }
}
