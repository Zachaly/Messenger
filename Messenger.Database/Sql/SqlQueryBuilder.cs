using Dapper;
using static Dapper.SqlMapper;

namespace Messenger.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly SqlBuilder _builder;
        private string _template = "";
        private string _table = "";
        private string _where = "";
        private DynamicParameters? _parameters;

        public SqlQueryBuilder()
        {
            _builder = new SqlBuilder();
        }

        public ISqlQueryBuilder AddPagination(int index, int size)
        {
            _template = $"SELECT /**select**/ FROM {_table} ORDER BY Id{_where} OFFSET {index * size} ROWS FETCH NEXT {size} ROWS ONLY";

            return this;
        }

        public (string Query, object? Params) Build()
        {
            var temp = _builder.AddTemplate(_template, _parameters);

            return (temp.RawSql, temp.Parameters);
        }

        public ISqlQueryBuilder Insert<T>(T entity) where T : class
        {
            _parameters = new DynamicParameters(entity);
            _parameters.RemoveUnused = false;

            var typeInfo = typeof(T);

            var properties = typeInfo.GetProperties();

            foreach(var prop in properties )
            {
                _parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            string columns = string.Join(", ", properties.Select(x => x.Name));

            string values = string.Join(", ", _parameters.ParameterNames.Select(param => $"@{param}"));

            _template = $"INSERT INTO {typeInfo.Name}({columns}) VALUES({values})";

            return this;
        }

        public ISqlQueryBuilder Select<T>(string table) where T : class
        {
            _template = $"SELECT /**select**/ FROM {table} ORDER BY Id";

            _table = table;

            foreach(var prop in typeof(T).GetProperties())
            {
                _builder.Select(prop.Name);
            }
            return this;
        }

        public ISqlQueryBuilder Where<T>(T request) where T : class
        {
            _parameters = new DynamicParameters(request);

            var typeInfo = typeof(T);

            foreach(var prop in typeInfo.GetProperties())
            {
                if(prop.GetValue(request) is not null)
                {
                    _builder.Where($"{prop.Name}=@{prop.Name}", _parameters);
                }
            }

            _where = " /**where**/";

            _template = $"{_template}{_where}";

            return this;
        }
    }
}
