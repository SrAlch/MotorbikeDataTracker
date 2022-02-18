using System;

namespace MotorbikeDataTracker.Api.Entities
{
    public class Motorbike
    {
        public Guid Id {get; set;}
        public string Brand {get; set;}
        public int Year {get; set;}
        public string Model {get; set;}
        public string Trim {get; set;}
        
    }
}