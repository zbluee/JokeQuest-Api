using JokeApi.Modles;

namespace JokeApi.Services;

public interface IJwtUtils {
    string Sign(User user);
    string Verify(string token);
}