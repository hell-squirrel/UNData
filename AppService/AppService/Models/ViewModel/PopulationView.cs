using Domain.Model;

namespace AppService.Models.ViewModel
{
    public class PopulationView
    {
        public int Indicator { get; set; }
        public int Frequency { get; set; }
        public int Age { get;set; }
        public int Sex { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}