using AppService.Interfaces;
using Domain.Model;

namespace AppService.Queries
{
    public class AuthenticateQuery: IQuery<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public AuthenticateQuery(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}