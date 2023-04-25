using Dapper;
using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;
using Messenger.Models;
using System.Reflection;
using System.Text;

namespace Messenger.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private SqlBuilder _builder;
        private string _pagination = "";
        private readonly StringBuilder _joinBuilder = new StringBuilder();
        private DynamicParameters? _parameters = new DynamicParameters();
        private string _orderBy = "";
        private bool _whereSet = false;

        public SqlQueryBuilder()
        {
            _builder = new SqlBuilder();
        }

        public (string Query, object Params) BuildSelect<T>(string table)
        {
            var typeInfo = typeof(T);

            var joinAttr = typeInfo.GetCustomAttributes<JoinAttribute>().Where(attr => !attr.Outside);
            var outsideJoins = typeInfo.GetCustomAttributes<JoinAttribute>().Where(attr => attr.Outside);

            foreach(var join in joinAttr)
            {
                _joinBuilder.Append(join.Statement);
                _joinBuilder.Append(' ');
            }

            var properties = typeInfo.GetProperties();

            foreach (var prop in properties)
            {
                var nameAttr = prop.GetCustomAttribute<SqlNameAttribute>();
                if (nameAttr is not null && nameAttr.JoinOutside)
                {
                    continue;
                }

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

            var template = $"SELECT /**select**/ FROM [{table}] {_joinBuilder}/**where**/ /**orderby**/{_pagination}";


            if(outsideJoins.Any())
            {
                var innerTemplate = _builder.AddTemplate(template, _parameters);

                _builder = new SqlBuilder();

                var outsideJoinBuilder = new StringBuilder();
                var namesBuilder = new StringBuilder();

                var outsideNames = properties
                    .Select(prop => new { Prop = prop, Attr = prop.GetCustomAttribute<SqlNameAttribute>() })
                    .Where(prop => prop.Attr is not null && prop.Attr.JoinOutside);

                foreach(var join in outsideJoins)
                {
                    outsideJoinBuilder.Append(join.Statement);
                }

                foreach(var prop in outsideNames)
                {
                    namesBuilder.Append(',');
                    namesBuilder.Append(prop.Attr.Name);
                }

                if (!string.IsNullOrEmpty(_orderBy))
                {
                    _builder.OrderBy($"t.{_orderBy}");
                }

                var t = $"SELECT t.*{namesBuilder} FROM ({innerTemplate.RawSql}) as t {outsideJoinBuilder} /**orderby**/";

                var temp = _builder.AddTemplate(t, _parameters);

                _builder = new SqlBuilder();

                return (temp.RawSql, temp.Parameters);
            } 
            else
            {
                var temp = _builder.AddTemplate(template, _parameters);

                _builder = new SqlBuilder();

                return (temp.RawSql, temp.Parameters);
            }
        }
        
        public (string Query, object Params) BuildCount(string table)
        {
            var template = $"SELECT COUNT(*) FROM [{table}] {_joinBuilder}/**where**/";

            var temp = _builder.AddTemplate(template, _parameters);

            _builder = new SqlBuilder();

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

            _builder = new SqlBuilder();

            return (temp.RawSql, temp.Parameters);
        }

        public ISqlQueryBuilder AddPagination(PagedRequest request)
        {
            int index = request.PageIndex ?? 0;
            int size = request.PageSize ?? 10;

            _pagination = $" OFFSET {index * size} ROWS FETCH NEXT {size} ROWS ONLY";

            return this;
        }

        public ISqlQueryBuilder Where<T>(T request) where T : class
        {
            _parameters = new DynamicParameters(request);

            var typeInfo = typeof(T);

            foreach(var prop in typeInfo.GetProperties().Where(prop => prop.GetValue(request) != default
                && prop.Name != "PageIndex" && prop.Name != "PageSize" && prop.GetCustomAttribute<SkipWhereAttribute>() is null))
            {
                var whereAttrib = prop.GetCustomAttribute<WhereAttribute>();
                if(whereAttrib is not null)
                {
                    var column = string.IsNullOrEmpty(whereAttrib.Column) ? $"[{prop.Name}]" : whereAttrib.Column;
                    var equal = $"[{prop.Name}]" == column ? "=" : "";
                    if(prop.PropertyType == typeof(string) && whereAttrib.ContentWrapper != "")
                    {
                        prop.SetValue(request, whereAttrib.ContentWrapper + (prop.GetValue(request) as string) + whereAttrib.ContentWrapper, null);
                    }
                    if (whereAttrib.Type == WhereType.AND)
                    {
                        _builder.Where($"{column}{equal}@{prop.Name}", _parameters);
                    } 
                    else
                    {
                        _builder.OrWhere($"({column}{equal}@{prop.Name})", _parameters);
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

            _whereSet = true;

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

        public (string Query, object Params) BuildDelete(string table)
        {
            var template = $"DELETE FROM [{table}] /**where**/";

            var temp = _builder.AddTemplate(template, _parameters);

            _builder = new SqlBuilder();

            return (temp.RawSql, temp.Parameters);
        }

        public (string Query, object Params) BuildSet<T>(T request, string table)
        {
            _parameters = new DynamicParameters(request);

            if (!_whereSet)
            {
                _builder.Where("[Id]=@Id", _parameters);
            }

            var setBuilder = new StringBuilder();

            var props = typeof(T).GetProperties().Where(x => x.Name != "Id" && x.GetValue(request) != default 
                && x.GetCustomAttribute<WhereAttribute>() is null);

            var index = 0;
            var count = props.Count();
            foreach(var prop in props) 
            {
                index++;
                var colon = index < count ? "," : "";
                setBuilder.Append($"[{table}].[{prop.Name}]=@{prop.Name}{colon} ");
            }

            var template = $"UPDATE [{table}] SET {setBuilder} /**where**/";

            var temp = _builder.AddTemplate(template, _parameters);

            _builder = new SqlBuilder();

            return (temp.RawSql, temp.Parameters);
        }
    }
}
