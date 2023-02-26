using JokeApi.Modles;
using JokeApi.Configs;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace JokeApi.Services;
public class UserServices : IUserServices
{
    private readonly IMongoCollection<User> _user;

    public UserServices(IOptions<MongoDBConfig> mongoDBConfig)
    {
        var clinet = new MongoClient(mongoDBConfig.Value.ConnectionString);
        var database = clinet.GetDatabase(mongoDBConfig.Value.DatabaseName);
        _user = database.GetCollection<User>(mongoDBConfig.Value.UserCollectionName);
    }

    public async Task<List<User>> GetAllAsync() => await _user.Find(_ => true).ToListAsync();
    public async Task DeleteAllAsync() => await _user.DeleteManyAsync(_ => true);
    public async Task<User> DeleteOneAsync(string userId)
    {
        var filter = Builders<User>.Filter.Where(user => user.Id == userId);
        var user = await _user.FindOneAndDeleteAsync(filter);
        return user;
    }
    public async Task CreateUser(User user) => await _user.InsertOneAsync(user);
    public async Task<User> FindUserByEmail(string email) => await _user.Find(user => user.Email == email).FirstOrDefaultAsync();
    public async Task<User> FindUserByIdProjected(string id) => await _user.Find(user => user.Id == id).Project<User>(Builders<User>.Projection.Exclude(user => user.Password)).FirstOrDefaultAsync();

    public async Task UpdateUserPoint(int point, string? id)
    {
        var filter = Builders<User>.Filter.Where(user => user.Id == id);
        var update = Builders<User>.Update.Inc(user => user.Point, point);
        await _user.FindOneAndUpdateAsync(filter, update);
    }

    public async Task AddJokeIdToAnsweredList(string? userId, string? jokeId){
        
        if(String.IsNullOrWhiteSpace(jokeId)) throw new ArgumentNullException("joke id");
        var filter = Builders<User>.Filter.Where(user => user.Id == userId);
        var update = Builders<User>.Update.Push<string>(user => user.AnsweredJokesId, jokeId);
        await _user.UpdateOneAsync(filter, update);
    }

    public async Task<User> FindUserById(string? id)
    {
        if (String.IsNullOrWhiteSpace(id)) throw new ArgumentNullException("userId");
        return await _user.Find(user => user.Id == id).FirstOrDefaultAsync();
    }
}