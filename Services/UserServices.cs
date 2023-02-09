using JokeApi.Modles;
using JokeApi.Configs;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace JokeApi.Services;
public class UserServices {

    private readonly IMongoCollection<User> _user;

    public UserServices(IOptions<MongoDBConfig> mongoDBConfig)
    {
        var clinet = new MongoClient(mongoDBConfig.Value.ConnectionString);
        var database = clinet.GetDatabase(mongoDBConfig.Value.DatabaseName);
        _user = database.GetCollection<User>(mongoDBConfig.Value.UserCollectionName);
    }

    public async Task<List<User>> GetAllAsync() => await _user.Find(_ => true).ToListAsync();
    public async Task DeleteAllAsync() => await _user.DeleteManyAsync(_ => true);
    public async Task<User> DeleteOneAsync(string userId) {
        var filter = Builders<User>.Filter.Where(user => user.Id == userId);
        var user = await _user.FindOneAndDeleteAsync(filter);
        return user;
    } 
    public async Task CreateUser(User user) => await _user.InsertOneAsync(user);
    public async Task<User> FindUserByEmail(string email) =>  await _user.Find(user => user.Email == email).FirstOrDefaultAsync();
    public async Task<User> FindUserByIdProjected(string id) =>  await _user.Find(user => user.Id == id).Project<User>(Builders<User>.Projection.Exclude(user => user.Password)).FirstOrDefaultAsync();
    public async Task<User> FindUserById(string id) =>  await _user.Find(user => user.Id == id).FirstOrDefaultAsync();
}