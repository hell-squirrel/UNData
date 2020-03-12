using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Population
    {
        [Key]
        public Guid Pointer { get; set; }
        public int Indicator { get; set; }
        public int Frequency { get; set; }
        public int Age { get;set; }
        public int Sex { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
}