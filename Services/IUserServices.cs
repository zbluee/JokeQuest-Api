using JokeApi.Modles;

namespace JokeApi.Services;

public interface IUserServices {
    public Task<List<User>> GetAllAsync();
    public Task DeleteAllAsync();
    public Task<User> DeleteOneAsync(string userId);
    public Task CreateUser(User user);
    public Task<User> FindUserByEmail(string email);
    public Task<User> FindUserByIdProjected(string id);
    public Task UpdateUserPoint(int point, string? id);
    public Task AddJokeIdToAnsweredList(string? userId, string? jokeId);
    public Task<User> FindUserById(string? id);
}