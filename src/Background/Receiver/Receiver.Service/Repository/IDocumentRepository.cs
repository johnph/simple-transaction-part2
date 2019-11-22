namespace Receiver.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDocumentRepository<TDocument> where TDocument : class
    {        
        Task<List<TDocument>> GetAllAsync();
        Task<List<TDocument>> GetAsync<TValue>(string field, TValue value);
        Task<List<TDocument>> GetAsync(int startingFrom, int count);
        Task<TDocument> GetAsync<TKey>(TKey id);
        Task InsertAsync(TDocument document);
        Task<bool> UpdateAsync<TKey, TValue>(TKey id, string field, TValue value);
        Task<bool> UpdateAsync<TKey>(TKey id, TDocument document);
        Task<bool> DeleteAsync<TKey>(TKey id);
        Task<bool> DeleteAsync<TValue>(string field, TValue value);
        Task<long> DeleteAllAsync();
        Task DropAsync();
        Task CreateAsync();
        Task<bool> AnyAsync<TValue>(string field, TValue value);
    }
}
