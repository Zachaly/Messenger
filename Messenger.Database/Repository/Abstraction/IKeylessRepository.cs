namespace Messenger.Database.Repository.Abstraction
{
    public interface IKeylessRepository<TEntity, TModel, TGetRequest>
    {
        Task InsertAsync(TEntity entity);
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<int> GetCount(TGetRequest request);
    }
}
