using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AppService.Providers.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AppService.Providers.Implementation
{
    public class UserProvider : IUserProvider
    {
        private readonly AppSettings _appSettings;
        private readonly IUser _user;

        public UserProvider(IOptions<AppSettings> appSettings, IUser user)
        {
            _appSettings = appSettings.Value;
            _user = user;
        }

        public User Authenticate(string username, string password)
        {
            var user = this._user.Get(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return WithoutPassword(user);
        }

        public IEnumerable<User> GetAll()
        {
            return WithoutPasswords(this._user.GetAll());
        }

        public User Create(User user)
        {
            return this._user.Create(user);
        }

        private IEnumerable<User> WithoutPasswords(IEnumerable<User> users)
        {
            return users.Select(WithoutPassword);
        }

        private User WithoutPassword(User user)
        {
            user.Password = null;
            return user;
        }
    }
}