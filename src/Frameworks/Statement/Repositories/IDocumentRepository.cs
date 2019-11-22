namespace Statement.Framework.Repositories
{
    using System.Threading.Tasks;

    public interface IDocumentRepository<TDocument> where TDocument : class
    {
        Task<TDocument> GetAsync<TKey>(TKey id);
    }
}
