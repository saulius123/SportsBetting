
namespace SportsBetting.Services.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> Create(string username, string password, string email);
        Task<string> Login(string username, string password);
    }
}
