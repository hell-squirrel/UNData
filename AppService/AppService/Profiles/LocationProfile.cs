using AppService.Models.ViewModel;
using AutoMapper;
using Domain.Model;

namespace AppService.Profiles
{
    public class LocationProfile: Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationView>();
        }
    }
}