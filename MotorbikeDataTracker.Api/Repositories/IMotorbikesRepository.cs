using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotorbikeDataTracker.Api.Entities;

namespace MotorbikeDataTracker.Api.Repositories
{
    public interface IMotorbikesRepository
    {
        Task<Motorbike> GetMotorbikeAsync(Guid Id);
        Task<IEnumerable<Motorbike>> GetMotorbikesAsync();
        Task CreateMotorbikeAsync(Motorbike motorbike);
        Task UpdateMotorbikeAsync(Motorbike motorbike);
        Task DeleteMotorbikeAsync(Guid Id);
    }
}