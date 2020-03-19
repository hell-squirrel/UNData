using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementation
{
    public class AnalyticsRepository : IAnalitics
    {
        private readonly Context _context;

        public AnalyticsRepository(Context context)
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
            if (string.IsNullOrWhiteSpace(name) || this._context.Locations.Any(l=>l.Name == name))
            {
                throw new ApplicationException("Name is invalid or already exist!");
            }
            
            if (this._context.Locations.Any(l => l.LocationId == locationId))
            {
                throw new ApplicationException("Location already exist!");
            }

            this._context.Locations.Add(new Location() {Name = name, LocationId = locationId});
            this._context.SaveChanges();
        }
    }
}