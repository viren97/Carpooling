using Carpooling.Models;

namespace Carpooling.Services {
    public interface IUserServices {
        User Login(string username, string password);
        User Register(string name, string contact, string address, string username, string password);
        bool IsUsernameAlreadyExist(string username);
    }
}
