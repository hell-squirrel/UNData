using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AppService.Queries.Handlers
{
    public class AuthenticateHandler: IQueryHandler<AuthenticateQuery,User>
    {
        private readonly AppSettings _appSettings;
        private readonly IUser _user;
        
        public AuthenticateHandler(IOptions<AppSettings> appSettings, IUser user)
        {
            _appSettings = appSettings.Value;
            _user = user;
        }
        
        public User Execute(AuthenticateQuery query)
        {
            var user = this._user.Get(x => x.Username == query.UserName && x.Password == query.Password);

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
        
        private User WithoutPassword(User user)
        {
            user.Password = null;
            return user;
        }
    }
}