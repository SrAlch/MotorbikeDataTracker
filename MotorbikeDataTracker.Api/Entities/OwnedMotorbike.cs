using System;

namespace MotorbikeDataTracker.Api.Entities
{
    public class OwnedMotorbike
    {
        public Guid Id { get; set; }
        public string NickName {get; set;}
        public Guid Vehicle {get; set;}
        public string Description {get; set;}
        public int OdoInit {get; set;}
        public int OdoCurrent {get; set;}
    }
}