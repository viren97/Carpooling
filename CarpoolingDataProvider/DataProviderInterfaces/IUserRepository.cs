using Carpooling.Models;
using System.Collections.Generic;

namespace Carpooling.DataProvider {
    public interface IUserRepository {
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        List<User> GetUsers();
        User GetUserById(int id);

    }
}
