using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GraphQL.API.Repository
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection ?? throw new ArgumentNullException(nameof(userCollection));
        }

        public IQueryable<User> GetAllUsers() => _userCollection.AsQueryable();

        public async Task<ILookup<string, User>> GetUsersByCountry(
            IReadOnlyList<string> countries, CancellationToken cancellationToken)
        {
            var filters = new List<FilterDefinition<User>>();
            foreach (var country in countries)
            {
                filters.Add(Builders<User>.Filter.Eq(u => u.Country, country));
            }

            var users = await _userCollection
                .Find(Builders<User>.Filter.Or(filters))
                .ToListAsync(cancellationToken);
            return users.ToLookup(t => t.Country);
        }

        public Task<User> GetUserAsync(User user, CancellationToken cancellationToken)
        {
            return _userCollection.Find(c => c.Id == user.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            return _userCollection.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        }

        public async Task<IReadOnlyDictionary<ObjectId, User>> GetUserAsync(
            IReadOnlyCollection<ObjectId> userIds,
            CancellationToken cancellationToken)
        {
            var filters = new List<FilterDefinition<User>>();
            foreach (var userId in userIds)
            {
                filters.Add(Builders<User>.Filter.Eq(u => u.Id, userId));
            }

            var users = await _userCollection
                .Find(Builders<User>.Filter.Or(filters))
                .ToListAsync(cancellationToken);
            return users.ToDictionary(t => t.Id);
        }
    }
}