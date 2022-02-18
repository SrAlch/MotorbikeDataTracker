using System;

namespace MotorbikeDataTracker.Api.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate{ get; set; }
        public string Email { get; set; }
        public Guid[] Vehicles { get; set; }
    }
}