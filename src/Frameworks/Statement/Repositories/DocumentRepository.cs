namespace Statement.Framework.Repositories
{
    using MongoDB.Driver;
    using System.Threading.Tasks;

    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private IMongoCollection<TDocument> _collection;

        public DocumentRepository(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public async Task<TDocument> GetAsync<TKey>(TKey id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
    }
}
