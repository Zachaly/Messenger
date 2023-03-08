using Dapper;
using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Messenger.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly SqlBuilder _builder;
        private string _pagination = "";
        private readonly StringBuilder _joinBuilder = new StringBuilder();
        private DynamicParameters? _parameters;
        private string _orderBy = "";

        public SqlQueryBuilder()
        {
            _builder = new SqlBuilder();
        }

        public (string Query, object Params) BuildSelect<T>(string table)
        {
            var typeInfo = typeof(T);

            var joinAttr = typeInfo.GetCustomAttribute<JoinAttribute>();

            if (joinAttr is not null)
            {
                _joinBuilder.Append(joinAttr.Statement);
                _joinBuilder.Append(' ');
            }

            foreach (var prop in typeInfo.GetProperties())
            {
                var nameAttr = prop.GetCustomAttribute<SqlNameAttribute>();
                if(nameAttr is not null)
                {
                    _builder.Select($"{nameAttr.Name} as [{prop.Name}]");
                } 
                else
                {
                    _builder.Select($"[{table}].[{prop.Name}]");
                }
            }

            if(!string.IsNullOrEmpty(_orderBy))
            {
                _builder.OrderBy($"[{table}].{_orderBy}");
            }

            var template = $"SELECT DISTINCT /**select**/ FROM [{table}] {_joinBuilder}/**where**/ /**orderby**/{_pagination}";

            var temp = _builder.AddTemplate(template, _parameters);

            return (temp.RawSql, temp.Parameters);
        }
        
        public (string Query, object Params) BuildCount(string table)
        {
            var template = $"SELECT COUNT(*) FROM [{table}] {_joinBuilder}/**where**/";

            var temp = _builder.AddTemplate(template, _parameters);

            return (temp.RawSql, temp.Parameters);
        }

        public (string Query, object Params) BuildInsert<T>(T entity, bool returnId = true)
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

            var output = returnId ? "OUTPUT INSERTED.[Id]" : "";

            var template = $"INSERT INTO [{typeInfo.Name}]({columns}) {output} VALUES({values})";

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
                else if (prop.GetCustomAttribute<JoinAttribute>() is null)
                {
                    _builder.Where($"[{prop.Name}]=@{prop.Name}", _parameters);
                }
                else
                {
                    _builder.Where($"[{prop.Name}]=@{prop.Name}");
                }
            }

            return this;
        }

        public ISqlQueryBuilder OrderBy(string columnName)
        {
            _orderBy = columnName;
            return this;
        }

        public ISqlQueryBuilder Join<T>(T request) where T : class
        {
            var joins = typeof(T)
                .GetProperties()
                .Select(prop => new { Attr = prop.GetCustomAttribute<JoinAttribute>(), Prop = prop })
                .Where(attr => attr.Attr is not null && attr.Prop.GetValue(request) != default);

            foreach(var join in joins)
            {
                _joinBuilder.Append(join.Attr.Statement);
                _joinBuilder.Append(' ');
                _parameters.Add($"@{join.Prop.Name}", join.Prop.GetValue(request));
            }

            return this;
        }
    }
}
