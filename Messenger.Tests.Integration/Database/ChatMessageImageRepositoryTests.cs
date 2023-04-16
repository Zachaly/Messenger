using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Integration.Database
{
    public class ChatMessageImageRepositoryTests : DatabaseTest
    {
        private readonly ChatMessageImageRepository _repository;

        public ChatMessageImageRepositoryTests() : base()
        {
            _repository = new ChatMessageImageRepository(_connectionFactory, new SqlQueryBuilder());
            _teardownQueries.Add("TRUNCATE TABLE [ChatMessageImage]");
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            var images = FakeDataFactory.CreateChatMessageImages(1, 5).ToList();
            images.AddRange(FakeDataFactory.CreateChatMessageImages(2, 5));

            await InsertImagesToDatabase(images);

            var image = (await GetAllFromDatabase<ChatMessageImage>("ChatMessageImage")).Last();

            var res = await _repository.GetByIdAsync(image.Id);

            Assert.Equal(image.FileName, res.FileName);
            Assert.Equal(image.MessageId, res.MessageId);
        }
    }
}
