using Messenger.Domain.Entity;
using Messenger.Models.Friend;
using Messenger.Models.Friend.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Database.Repository
{
    public class FriendRepository : IFriendRepository
    {
        public Task<IEnumerable<FriendListItem>> GetAllFriendsAsync(GetFriendsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<long> InsertFriendAsync(Friend friend)
        {
            throw new NotImplementedException();
        }
    }
}
