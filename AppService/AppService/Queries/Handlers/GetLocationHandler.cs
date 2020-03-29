using Domain.Interfaces;
using Domain.Model;

namespace AppService.Queries.Handlers
{
    public class GetLocationHandler:IQueryHandler<GetLocationQuery,Location>
    {
        private readonly IAnalitics _analitics;
        
        public GetLocationHandler(IAnalitics analitics)
        {
            _analitics = analitics;
        }
        
        public Location Execute(GetLocationQuery query)
        {
            return this._analitics.GetLocation(query.LocationId);
        }
    }
}