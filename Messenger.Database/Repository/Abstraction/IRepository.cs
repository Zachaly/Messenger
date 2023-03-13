namespace Messenger.Database.Repository.Abstraction
{
    public interface IRepository<TEntity, TModel, TGetRequest> : IKeylessRepository<TEntity, TModel, TGetRequest>
    {
        Task DeleteByIdAsync(long id);
        new Task<long> InsertAsync(TEntity entity);
    }
}
