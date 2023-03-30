using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Sql;
using Messenger.Models;

namespace Messenger.Database.Repository.Abstraction
{
    public abstract class RepositoryBase<TEntity, TModel, TGetRequest> 
        : KeylessRepositoryBase<TEntity, TModel, TGetRequest>, IRepository<TEntity, TModel, TGetRequest>
        where TEntity : class
        where TGetRequest : PagedRequest
        where TModel : class
    {

        protected RepositoryBase(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder)
            : base(connectionFactory, sqlQueryBuilder) 
        {
            DefaultOrderBy = "[Id]";
        }

        public virtual Task DeleteByIdAsync(long id)
        {
            var query = _sqlQueryBuilder.Where(new { Id = id }).BuildDelete(Table);

            return ExecuteQueryAsync(query.Query, query.Params);
        }

        public override Task<long> InsertAsync(TEntity entity)
        {
                                                                                                                 var query = _sqlQueryBuilder
                .BuildInsert(entity);

            return QuerySingleAsync<long>(query.Query, query.Params);
        }
    }
}
