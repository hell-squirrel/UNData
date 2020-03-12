using AppService.Models.ViewModel;
using AutoMapper;
using Domain.Model;

namespace AppService.Profiles
{
    public class PopulationProfile:Profile
    {
        public PopulationProfile()
        {
            CreateMap<Population, PopulationView>();
        }
    }
}