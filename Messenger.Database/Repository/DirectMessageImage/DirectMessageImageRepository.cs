using Messenger.Database.Connection;
using Messenger.Database.Repository.Abstraction;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;
using Messenger.Models.DirectMessageImage.Request;

namespace Messenger.Database.Repository
{
    public class DirectMessageImageRepository : RepositoryBase<DirectMessageImage, DirectMessageImage, GetDirectMessageImageRequest>, IDirectMessageImageRepository
    {
        public DirectMessageImageRepository(IConnectionFactory connectionFactory, ISqlQueryBuilder sqlQueryBuilder) 
            : base(connectionFactory, sqlQueryBuilder)
        {
            Table = "DirectMessageImage";
        }

        public Task<DirectMessageImage> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
