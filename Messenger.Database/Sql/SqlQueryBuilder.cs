﻿using Dapper;

namespace Messenger.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly SqlBuilder _builder;

        public SqlQueryBuilder()
        {
            _builder = new SqlBuilder();
        }

        public ISqlQueryBuilder AddPagination(int index, int size)
        {
            throw new NotImplementedException();
        }

        public (string Query, object? Params) Build()
        {
            throw new NotImplementedException();
        }

        public ISqlQueryBuilder Insert<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public ISqlQueryBuilder Select<T>(string table) where T : class
        {
            throw new NotImplementedException();
        }

        public ISqlQueryBuilder Where<T>(T request) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
