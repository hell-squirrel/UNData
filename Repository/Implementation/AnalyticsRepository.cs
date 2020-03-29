using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public Location GetLocation(int locationId)
        {
            return this._context.Locations.AsNoTracking().FirstOrDefault(x => x.LocationId == locationId);
        }

        public Location AddDescription(int locationId, string description)
        {
            var location = this._context.Locations.AsNoTracking().FirstOrDefault(x => x.LocationId == locationId);
            if (location == null)
            {
                throw new ApplicationException("Cannot add location description.");
            }

            location.Description = description;
            this._context.Update(location);
            this._context.SaveChanges();
            return location;
        }
    }
}