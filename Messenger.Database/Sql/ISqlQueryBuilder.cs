namespace Messenger.Database.Sql
{
    public interface ISqlQueryBuilder
    {
        ISqlQueryBuilder Select<T>() where T : class;
        ISqlQueryBuilder Insert<T>(T entity) where T : class;
        ISqlQueryBuilder AddPagination(int index, int size);
        ISqlQueryBuilder Where<T>(T request) where T : class;
        (string Query, object? Params) Build();
    }
}
