using Dapper;
using Messenger.Database.Connection;
using Messenger.Database.Sql;
using Messenger.Models;

namespace Messenger.Database.Repository.Abstraction
{
    public abstract class KeylessRepositoryBase<TEntity, TModel, TGetRequest> : IKeylessRepository<TEntity, TModel, TGetRequest>
        where TEntity : class
        where TGetRequest : PagedRequest
        where TModel : class
    {
        protected readonly IConnectionFactory _connectionFactory;
        protected readonly ISqlQueryBuilder _sqlQueryBuilder;
        protected string Table { get; set; } = "";
        protected string DefaultOrderBy { get; set; } = "";

        protected KeylessRepositoryBase(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder)
        {
            _connectionFactory = connectionFactory;
            _sqlQueryBuilder = sqlQueryBuilder;
        }

        protected async Task ExecuteQueryAsync(string query, object? param)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                await connection.QueryAsync(query, param);
            }
        }

        protected async Task<T> QuerySingleAsync<T>(string query, object? param)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(query, param);
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string query, object? param)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                return await connection.QueryAsync<T>(query, param);
            }
        }

        public virtual Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            var query = _sqlQueryBuilder.Join(request)
                .Where(request)
                .OrderBy(DefaultOrderBy)
                .AddPagination(request)
                .BuildSelect<TModel>(Table);

            return QueryAsync<TModel>(query.Query, query.Params);
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            var query = _sqlQueryBuilder
                .BuildInsert(entity, false);

            return ExecuteQueryAsync(query.Query, query.Params);
        }

        public virtual Task<int> GetCount(TGetRequest request)
        {
            var query = _sqlQueryBuilder
                .Where(request)
                .BuildCount(Table);

            return QuerySingleAsync<int>(query.Query, query.Params);
        }
    }
}
