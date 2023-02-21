using JokeApi.Configs;
using JokeApi.Modles;
using JokeApi.Errors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

namespace JokeApi.Services;

public class JwtUtils : IJwtUtils
{
    
    private readonly byte[] _signingKey;
    private readonly ILogger<JwtUtils> _logger;
    public JwtUtils(IOptions<JwtConfig> jwtSettings, ILogger<JwtUtils> logger)
    {
        _signingKey = Encoding.ASCII.GetBytes(jwtSettings.Value.Secret);
        _logger = logger;
    }

    public string Sign(User user)
    {
        // generate a user-specific jwt token that is valid for 15 days
        if(user.Id == null || user.Email == null || user.Name == null) throw new NullReferenceException("user name and email are required");
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim> {
            new Claim("id", user.Id),
            new Claim(ClaimTypes.Name, user.Name), 
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string Verify(string token)
    {
        if(String.IsNullOrWhiteSpace(token)) throw new NullReferenceException("token is required");
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_signingKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validateToken);

            var jwtToken = (JwtSecurityToken)validateToken;
            var userId = jwtToken.Claims.First(claim => claim.Type == "id").ToString().Split(" ").Last();
            return userId;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new BadRequestException("unable to verify the token");
        }

    }
}
