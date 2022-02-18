using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeDataTracker.Api.Entities;

namespace MotorbikeDataTracker.Api.Repositories
{
    public class InMemItemsRepository
    {
        private readonly List<Motorbike> Motorbikes = new()
        {
            new Motorbike { Id = Guid.NewGuid(), Brand = "Yamaha", Year = 2020, Model = "R3", Trim = "DeepBlue"},
            new Motorbike { Id = Guid.NewGuid(), Brand = "Ducati", Year = 2020, Model = "Panigale V2", Trim = "Rosso Corse"},
            new Motorbike { Id = Guid.NewGuid(), Brand = "Honda", Year = 2020, Model = "CBR650R", Trim = "Basic"}
        };

        public async Task<IEnumerable<Motorbike>> GetItemsAsync()
        {
            return await Task.FromResult(Motorbikes);
        }

        public async Task<Motorbike> GetItemAsync(Guid id)
        {
            var item = Motorbikes.Where(item => item.Id == id).SingleOrDefault();
            return await Task.FromResult(item);
        }
    
        public async Task CreateItemAsync(Motorbike item)
        {
            Motorbikes.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Motorbike item)
        {
            var index = Motorbikes.FindIndex(existingItem => existingItem.Id == item.Id);
            Motorbikes[index] = item;
            await Task.CompletedTask;

        }

        public async Task DeleteItemAsync(Guid id)
        {
            var index = Motorbikes.FindIndex(existingItem => existingItem.Id == id);
            Motorbikes.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}