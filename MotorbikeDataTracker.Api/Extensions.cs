using MotorbikeDataTracker.Api.Dtos;
using MotorbikeDataTracker.Api.Entities;

namespace MotorbikeDataTracker.Api
{
    public static class Extension
    {
        public static MotorbikeDto AsDto(this Motorbike motorbike)
        {
            return new MotorbikeDto(motorbike.Id, motorbike.Brand, motorbike.Year, motorbike.Model, motorbike.Trim);
        }
    }
}