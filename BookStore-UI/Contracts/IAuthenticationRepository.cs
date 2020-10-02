using System.Threading.Tasks;
using BookStore_UI.Models;

namespace BookStore_UI.Contracts {
    public interface IAuthenticationRepository {
        public Task<bool> Register(RegistrationModel user);
        public Task<bool> Login(LoginModel login);
        public Task Logout();
    }
}