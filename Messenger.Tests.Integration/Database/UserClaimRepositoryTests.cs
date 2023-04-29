using Messenger.Database.Repository;
using Messenger.Database.Sql;
using Messenger.Domain.Entity;

namespace Messenger.Tests.Integration.Database
{
    public class UserClaimRepositoryTests : DatabaseTest
    {
        private readonly UserClaimRepository _repository;

        public UserClaimRepositoryTests() : base() 
        {
            _teardownQueries.Add("TRUNCATE TABLE [UserClaim]");
            _repository = new UserClaimRepository(_connectionFactory, new SqlQueryBuilder());
        }

        [Fact]
        public async Task DeleteAsync()
        {
            const long UserId = 2;
            const string Claim = "claim to delete";

            var claimsToAdd = new string[] { Claim, "claim1", "claim3" };

            await InsertUserClaimsToDatabase(FakeDataFactory.CreateUserClaims(1, claimsToAdd));
            await InsertUserClaimsToDatabase(FakeDataFactory.CreateUserClaims(UserId, claimsToAdd));

            await _repository.DeleteAsync(UserId, Claim);

            var claims = await GetAllFromDatabase<UserClaim>("UserClaim");

            Assert.Equal(5, claims.Count());
            Assert.DoesNotContain(claims, x => x.UserId == UserId && x.Value == Claim);
        }
    }
}
