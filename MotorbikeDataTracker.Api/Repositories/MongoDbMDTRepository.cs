using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MotorbikeDataTracker.Api.Entities;

namespace MotorbikeDataTracker.Api.Repositories
{
    public class MongoDbMDTRepository : IMotorbikesRepository
    {
        private const string databaseName = "motorbikesDB";
        private const string collectionName = "motorbikes";
        private readonly IMongoCollection<Motorbike> motorbikesCollection;
        private readonly FilterDefinitionBuilder<Motorbike> filterBuilder = Builders<Motorbike>.Filter;
        public MongoDbMDTRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            motorbikesCollection = database.GetCollection<Motorbike>(collectionName);
        }

        public async Task CreateMotorbikeAsync(Motorbike motorbike)
        {
            await motorbikesCollection.InsertOneAsync(motorbike);
        }

        public async Task DeleteMotorbikeAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(motorbike => motorbike.Id, Id);
            await motorbikesCollection.DeleteOneAsync(filter);
        }

        public async Task<Motorbike> GetMotorbikeAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(motorbike => motorbike.Id, Id);
            return await motorbikesCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Motorbike>> GetMotorbikesAsync()
        {
            return await motorbikesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateMotorbikeAsync(Motorbike motorbike)
        {
            var filter = filterBuilder.Eq(existingMotorbike => existingMotorbike.Id, motorbike.Id);
            await motorbikesCollection.ReplaceOneAsync(filter, motorbike);
        }
    }
}