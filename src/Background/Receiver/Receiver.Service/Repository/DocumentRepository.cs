using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver.Service.Repository
{
    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private IMongoCollection<TDocument> _collection;

        public DocumentRepository(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public async Task InsertAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task<TDocument> GetAsync<TKey>(TKey id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<TDocument>> GetAllAsync()
        {
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<TDocument>> GetAsync<TValue>(string field, TValue value)
        {
            var filter = Builders<TDocument>.Filter.Eq(field, value);
            var result = await _collection.Find(filter).ToListAsync();

            return result;
        }

        public async Task<List<TDocument>> GetAsync(int startingFrom, int count)
        {
            var result = await _collection.Find(new BsonDocument())
            .Skip(startingFrom)
            .Limit(count)
            .ToListAsync();

            return result;
        }

        public async Task<bool> UpdateAsync<TKey, TValue>(TKey id, string field, TValue value)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var update = Builders<TDocument>.Update.Set(field, value);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount != 0;
        }

        public async Task<bool> UpdateAsync<TKey>(TKey id, TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var result = await _collection.ReplaceOneAsync(filter, document, new UpdateOptions() { IsUpsert = true });
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync<TKey>(TKey id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<bool> DeleteAsync<TValue>(string field, TValue value)
        {
            var filter = Builders<TDocument>.Filter.Eq(field, value);
            var result = await _collection.DeleteOneAsync(filter);
            return result.IsAcknowledged;
        }

        public async Task<long> DeleteAllAsync()
        {
            var filter = new BsonDocument();
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public async Task DropAsync()
        {
            await _collection.Database.DropCollectionAsync(typeof(TDocument).Name.ToLower());
        }

        public async Task CreateAsync()
        {
            await _collection.Database.CreateCollectionAsync(typeof(TDocument).Name.ToLower());
        }

        public async Task<bool> AnyAsync<TValue>(string field, TValue value)
        {
            var filter = Builders<TDocument>.Filter.Eq(field, value);
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
