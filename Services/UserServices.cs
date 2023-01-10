using JobServices.Modles;
using JobServices.Configs;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

public class UserServices {

    private readonly IMongoCollection<User> _user;

    public UserServices(IOptions<MongoDBConfig> mongoDBConfig)
    {
        var clinet = new MongoClient(mongoDBConfig.Value.ConnectionString);
        var database = clinet.GetDatabase(mongoDBConfig.Value.DatabaseName);
        _user = database.GetCollection<User>(mongoDBConfig.Value.UserCollectionName);
    }

    public async Task<List<User>> GetAllAsync() => await _user.Find(_ => true).ToListAsync();
    public async Task CreateUser(User user) => await _user.InsertOneAsync(user);
    public async Task<User> FindUserByEmail(string email) =>  await _user.Find(user => user.Email == email).FirstOrDefaultAsync();
}