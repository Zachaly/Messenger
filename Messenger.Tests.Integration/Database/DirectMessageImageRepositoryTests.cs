using Dapper;
using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;

namespace Messenger.Tests.Integration.Database
{
    public class DirectMessageImageRepositoryTests : DatabaseTest
    {
        private readonly DirectMessageImageRepository _repository;

        public DirectMessageImageRepositoryTests() : base()
        {
            _teardownQueries.Add("TRUNCATE TABLE [DirectMessageImage]");
            _repository = new DirectMessageImageRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            var images = FakeDataFactory.CreateMessageImages(1, 5).ToList();
            images.AddRange(FakeDataFactory.CreateMessageImages(2, 5));

            await InsertImagesToDatabase(images);

            var image = (await GetAllFromDatabase<DirectMessageImage>("DirectMessageImage")).Last();

            var res = await _repository.GetByIdAsync(image.Id);

            Assert.Equal(image.FileName, res.FileName);
            Assert.Equal(image.MessageId, res.MessageId);
        }
    }
}
