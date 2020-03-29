using System.Collections.Generic;
using AppService.Models.ViewModel;
using Domain.Model;

namespace AppService.Providers.Interfaces
{
    public interface ILocationProvider
    {
        IEnumerable<Location> Search(string query, int page = 1, int pageSize = 5);

        void SaveLocation(int locationId, string locationName);
        
        Location AddDescription(int locationId, string description);

        Location Get(int locationId);
    }
}