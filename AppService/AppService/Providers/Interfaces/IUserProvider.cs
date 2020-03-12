using System.Collections.Generic;
using Domain.Model;

namespace AppService.Providers.Interfaces
{
    public interface IUserProvider
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User Create(User user);
    }
}