using AppService.Interfaces;
using Domain.Model;

namespace AppService.Queries
{
    public class GetLocationQuery:  IQuery<Location>
    {
        public int LocationId { get; }

        public GetLocationQuery(int locationId)
        {
            LocationId = locationId;
        }
    }
}