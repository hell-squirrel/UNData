using AppService.Models;
using AppService.Models.ViewModel;
using AutoMapper;
using Domain.Model;

namespace AppService.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User,UserView>();
            CreateMap<CreateUserModel,User>();
        }
        
    }
}