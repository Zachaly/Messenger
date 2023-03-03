namespace Messenger.Database.Sql
{
    public interface ISqlQueryBuilder
    {
        ISqlQueryBuilder AddPagination(int index, int size);
        ISqlQueryBuilder Where<T>(T request) where T : class;
        (string Query, object Params) BuildInsert<T>(T entity);
        (string Query, object Params) BuildSelect<T>(string table);
    }
}
