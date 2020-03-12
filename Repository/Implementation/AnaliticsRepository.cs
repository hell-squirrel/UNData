using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementation
{
    public class AnaliticsRepository : IAnalitics
    {
        private readonly Context _context;

        public AnaliticsRepository(Context context)
        {
            this._context = context;
        }

        public IList<Population> GetData(int location)
        {
            return this._context.Population.Where(p => p.LocationId == location).ToList();
        }

        public void SavePopulation(IEnumerable<Population> dataset)
        {
            this._context.Population.AddRange(dataset);
            this._context.SaveChanges();
        }

        public void SaveLocation(int locationId, string name)
        {
            if (this._context.Locations.Any(l => l.LocationId == locationId))
            {
                return;
            }

            this._context.Locations.Add(new Location() {Name = name, LocationId = locationId});
            this._context.SaveChanges();
        }
    }
}