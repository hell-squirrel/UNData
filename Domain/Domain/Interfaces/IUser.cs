using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Model;

namespace Domain.Interfaces
{
    public interface IUser
    {
        IList<User> GetAll();
        User Create(User newuser);
        bool IsExist(string username, string password);
        User Get(Func<User, bool> predicate);
    }
}