namespace Messenger.Database.Sql
{
    public interface ISqlQueryBuilder
    {
        ISqlQueryBuilder AddPagination(int index, int size);
        ISqlQueryBuilder Where<T>(T request) where T : class;
        (string Query, object Params) BuildInsert<T>(T entity, bool returnId = true);
        (string Query, object Params) BuildSelect<T>(string table);
        ISqlQueryBuilder Join<T>(T request) where T : class;
        ISqlQueryBuilder OrderBy(string columnName);
        (string Query, object Params) BuildCount(string table);
        (string Query, object Params) BuildDelete(string table);
    }
}
