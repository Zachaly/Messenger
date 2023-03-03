using Messenger.Database.Sql;
using Messenger.Domain.Enum;
using Messenger.Domain.SqlAttributes;

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
    
    class JoinTestModel
    {
        public int Id { get; set; }
        [Join(Column = JoinColumn, Table = JoinTable, Statement = JoinStatement)]
        public int Name { get; set; }

        public const string JoinColumn = "Name";
        public const string JoinTable = "Table";
        public const string JoinStatement = "JOIN ON [Table].[Id]=[OtherTable].[Id]";
    }

    class WhereTestRequest
    {
        [Where(Column = "[Name] LIKE ", Type = WhereType.OR)]
        public string Name { get; set; }
        [Where(Type = WhereType.OR)]
        public string Something { get; set; }
    }

    public class SqlQueryBuilderTests
    {
        private readonly SqlQueryBuilder _builder;

        public SqlQueryBuilderTests()
        {
            _builder = new SqlQueryBuilder();
        }

        private string FormatQuery(string query)
            => query.Replace("\n", "").Replace("  ", " ");

        [Fact]
        public void Build_Select()
        {
            const string Table = "table";
            var res = _builder.BuildSelect<TestEntity>(Table);

            Assert.Equal($"SELECT [Id] , [Name] FROM [{Table}] ORDER BY [{Table}].[Id]", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Insert()
        {
            var testEntity = new TestEntity();

            var res = _builder.BuildInsert(testEntity);

            Assert.Equal("INSERT INTO [TestEntity]([Name]) OUTPUT INSERTED.[Id] VALUES(@Name)", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Select_Where()
        {
            const string Table = "table";
            var request = new TestRequest { Id = 2137, Name = "test" };
            var res = _builder.Where(request).BuildSelect<TestEntity>(Table);

            Assert.Equal($"SELECT [Id] , [Name] FROM [{Table}] WHERE [Id]=@Id AND [Name]=@Name ORDER BY [{Table}].[Id]", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Select_Where_Skip_Nulls()
        {
            const string Table = "table";
            var request = new TestRequest { Id = 2137, Name = null };
            var res = _builder.Where(request).BuildSelect<TestEntity>(Table);

            Assert.Equal($"SELECT [Id] , [Name] FROM [{Table}] WHERE [Id]=@Id ORDER BY [{Table}].[Id]", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Select_With_Pagination()
        {
            const string Table = "table";
            const int PageSize = 10;
            const int PageIndex = 0;
            var res = _builder.AddPagination(PageIndex, PageSize).BuildSelect<TestEntity>(Table);

            Assert.Equal($"SELECT [Id] , [Name] FROM [{Table}] ORDER BY [{Table}].[Id] OFFSET {PageIndex * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Select_With_Join()
        {
            const string Table = "table";

            var res = _builder.BuildSelect<JoinTestModel>(Table);

            Assert.Equal($"SELECT [Id] , [{JoinTestModel.JoinTable}].[{JoinTestModel.JoinColumn}] as [Name] FROM [{Table}] {JoinTestModel.JoinStatement} ORDER BY [{Table}].[Id]", FormatQuery(res.Query));
        }

        [Fact]
        public void Build_Select_Where_With_Attribute()
        {
            const string Table = "table";

            var request = new WhereTestRequest
            {
                Name = "name",
                Something = "som"
            };

            var res = _builder.Where(request).BuildSelect<TestEntity>(Table);

            var r = 1;

            Assert.Equal($"SELECT [Id] , [Name] FROM [{Table}] WHERE ( [Name] LIKE @Name OR [Something]=@Something ) ORDER BY [{Table}].[Id]", FormatQuery(res.Query));
        }
    }
}
