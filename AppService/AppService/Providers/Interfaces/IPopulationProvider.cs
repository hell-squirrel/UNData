using System;
using System.Collections.Generic;
using Domain.Model;

namespace AppService.Providers.Interfaces
{
    public interface IPopulationProvider
    {
        public IList<Population> GetData(int location);

        public void LoadPopulation(DateTime startData,DateTime endDate);
    }
}