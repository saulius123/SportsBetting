using SportsBetting.Data.Models;
using System.Threading.Tasks;

namespace SportsBetting.Services.Interfaces
{
    public interface IResultService
    {
        Task CreateResultAsync(Result result);
        Task<Result?> GetResultByIdAsync(int id);
    }
}