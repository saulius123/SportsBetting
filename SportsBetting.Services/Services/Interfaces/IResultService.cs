using SportsBetting.Data.Models;

namespace SportsBetting.Services.Services.Interfaces
{
    public interface IResultService
    {
        Task CreateResultAsync(Result result);
        Task<Result?> GetResultByIdAsync(int id);
    }
}