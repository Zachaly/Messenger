using Dapper;

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
            _template = $"SELECT /**select**/ FROM {_table}{_where} ORDER BY [Id] OFFSET {index * size} ROWS FETCH NEXT {size} ROWS ONLY";

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

            foreach(var prop in properties)
            {
                if(prop.Name == "Id")
                {
                    continue;
                }
                _parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            string columns = string.Join(", ", properties.Where(x => x.Name != "Id").Select(x => $"[{x.Name}]"));

            string values = string.Join(", ", _parameters.ParameterNames.Select(param => $"@{param}"));

            _template = $"INSERT INTO [{typeInfo.Name}]({columns}) OUTPUT INSERTED.[Id] VALUES({values})";

            return this;
        }

        public ISqlQueryBuilder Select<T>(string table) where T : class
        {
            _template = $"SELECT /**select**/ FROM [{table}] ORDER BY [Id]";

            _table = $"[{table}]";

            foreach(var prop in typeof(T).GetProperties())
            {
                _builder.Select($"[{prop.Name}]");
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
                    _builder.Where($"[{prop.Name}]=@{prop.Name}", _parameters);
                }
            }

            _where = "/**where**/";

            _template = _template.Replace("ORDER BY [Id]", "");

            _template = $"{_template}{_where} ORDER BY [Id]";

            return this;
        }
    }
}
