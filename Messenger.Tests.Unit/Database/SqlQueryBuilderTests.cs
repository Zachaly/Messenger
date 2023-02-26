using Messenger.Database.Sql;

namespace Messenger.Tests.Unit.Database
{
    class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class TestRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    public class SqlQueryBuilderTests
    {
        private readonly SqlQueryBuilder _builder;

        public SqlQueryBuilderTests()
        {
            _builder = new SqlQueryBuilder();
        }

        [Fact]
        public void Build_Select()
        {
            const string Table = "table";
            var res = _builder.Select<TestEntity>(Table).Build();

            Assert.Equal($"SELECT Id , Name FROM {Table} ORDER BY Id", res.Query.Replace("\n", ""));
        }

        [Fact]
        public void Build_Insert()
        {
            var testEntity = new TestEntity();

            var res = _builder.Insert(testEntity).Build();

            Assert.Equal("INSERT INTO TestEntity(Id, Name) VALUES(@Id, @Name)", res.Query.Replace("\n", ""));
        }

        [Fact]
        public void Build_Select_Where()
        {
            const string Table = "table";
            var request = new TestRequest { Id = 2137, Name = "test" };
            var res = _builder.Select<TestEntity>(Table).Where(request).Build();

            Assert.Equal($"SELECT Id , Name FROM {Table} ORDER BY Id WHERE Id=@Id AND Name=@Name", res.Query.Replace("\n", ""));
        }

        [Fact]
        public void Build_Select_Where_Skip_Nulls()
        {
            const string Table = "table";
            var request = new TestRequest { Id = 2137, Name = null };
            var res = _builder.Select<TestEntity>(Table).Where(request).Build();

            Assert.Equal($"SELECT Id , Name FROM {Table} ORDER BY Id WHERE Id=@Id", res.Query.Replace("\n", ""));
        }

        [Fact]
        public void Build_Select_With_Pagination()
        {
            const string Table = "table";
            const int PageSize = 10;
            const int PageIndex = 0;
            var res = _builder.Select<TestEntity>(Table).AddPagination(PageIndex, PageSize).Build();

            Assert.Equal($"SELECT Id , Name FROM {Table} ORDER BY Id OFFSET {PageIndex * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY", res.Query.Replace("\n", ""));
        }
    }
}
