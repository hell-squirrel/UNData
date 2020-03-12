using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementation
{
    public class UserRepository:IUser
    {
        private readonly Context _context;
        
        public UserRepository(Context context)
        {
            this._context = context;
        }
        
        public IList<User> GetAll()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User Create(User newUser)
        {
            var user  = this._context.Users.Add(newUser);
            this._context.SaveChanges();
            return user.Entity;
        }

        public bool IsExist(string username, string password)
        {
            return this._context.Users.Any(x => x.Username == username && x.Password == password);
        }

        public User Get(Func<User, bool> predicate)
        {
            return this._context.Users.AsNoTracking().SingleOrDefault(predicate);
        }
    }
}