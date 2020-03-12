using System.Collections;
using System.Collections.Generic;
using Domain.Model;

namespace Domain.Interfaces
{
    public interface IAnalitics
    {
        public IList<Population> GetData(int location);

        public void SavePopulation(IEnumerable<Population> dataset);

        public void SaveLocation(int locationId, string name);
    }
}