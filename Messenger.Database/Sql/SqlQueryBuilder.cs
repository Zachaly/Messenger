using Dapper;
using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;
using System.Reflection;
using System.Text;

namespace Messenger.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly SqlBuilder _builder;
        private string _pagination = "";
        private readonly StringBuilder _joinBuilder = new StringBuilder();
        private DynamicParameters? _parameters;

        public SqlQueryBuilder()
        {
            _builder = new SqlBuilder();
        }

        public (string Query, object Params) BuildSelect<T>(string table)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                var joinAttr = prop.GetCustomAttribute<JoinAttribute>();
                if(joinAttr is not null)
                {
                    _builder.Select($"[{joinAttr.Table}].[{joinAttr.Column}] as [{prop.Name}]");
                    _joinBuilder.Append(joinAttr.Statement);
                    _joinBuilder.Append(' ');
                } 
                else
                {
                    _builder.Select($"[{prop.Name}]");
                }
            }

            var template = $"SELECT /**select**/ FROM [{table}] {_joinBuilder}/**where**/ ORDER BY [{table}].[Id]{_pagination}";

            var temp = _builder.AddTemplate(template, _parameters);

            return (temp.RawSql, temp.Parameters);
        }

        public (string Query, object Params) BuildInsert<T>(T entity)
        {
            var parameters = new DynamicParameters(entity);
            parameters.RemoveUnused = false;

            var typeInfo = typeof(T);

            var properties = typeInfo.GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name == "Id")
                {
                    continue;
                }
                parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            string columns = string.Join(", ", properties.Where(x => x.Name != "Id").Select(x => $"[{x.Name}]"));

            string values = string.Join(", ", parameters.ParameterNames.Select(param => $"@{param}"));

            var template = $"INSERT INTO [{typeInfo.Name}]({columns}) OUTPUT INSERTED.[Id] VALUES({values})";

            var temp = _builder.AddTemplate(template, parameters);

            return (temp.RawSql, temp.Parameters);
        }

        public ISqlQueryBuilder AddPagination(int index, int size)
        {
            _pagination = $" OFFSET {index * size} ROWS FETCH NEXT {size} ROWS ONLY";

            return this;
        }

        public ISqlQueryBuilder Where<T>(T request) where T : class
        {
            _parameters = new DynamicParameters(request);

            var typeInfo = typeof(T);

            foreach(var prop in typeInfo.GetProperties().Where(prop => prop.GetValue(request) != default))
            {
                var whereAttrib = prop.GetCustomAttribute<WhereAttribute>();
                if(whereAttrib is not null)
                {
                    var column = string.IsNullOrEmpty(whereAttrib.Column) ? $"[{prop.Name}]" : whereAttrib.Column;
                    var equal = $"[{prop.Name}]" == column ? "=" : "";
                    if (whereAttrib.Type == WhereType.AND)
                    {
                        _builder.Where($"{column}{equal}@{prop.Name}", _parameters);
                    } 
                    else
                    {
                        _builder.OrWhere($"{column}{equal}@{prop.Name}", _parameters);
                    }
                }
                else
                {
                    _builder.Where($"[{prop.Name}]=@{prop.Name}", _parameters);
                }
            }

            return this;
        }
    }
}
