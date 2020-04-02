using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Model
{
    public class Population
    {
        [Key] 
        public Guid Pointer { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        public int Indicator { get; set; }
        public int Frequency { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public int LocationId { get; set; }
        [ForeignKey("LocationId")] 
        public Location Location { get; set; }
    }
}