using SportsBetting.Data.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SportsBetting.Services.Services.Interfaces;
using SportsBetting.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public IdentityService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<string?> Login(string username, string password)
    {
        var user = await _userRepository.GetUser(username);

        if (user == null || !VerifyPassword(password, user.Password))
        {
            return null;
        }
        return GenerateJwtToken(user);
    }

    public async Task<bool> Create(string username, string password, string email)
    {
        if (await _userRepository.UserExists(username))
        {
            return false;
        }

        var userToCreate = new User
        {
            Username = username,
            Password = HashPassword(password),
            Email = email,
        };

        await _userRepository.Create(userToCreate, password);

        return true;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            }),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        bool verificationResult = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        return verificationResult;
    }

    private string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hashedPassword;
    }
}