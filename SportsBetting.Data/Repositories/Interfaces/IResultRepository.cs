using SportsBetting.Data.Models;
using System.Threading.Tasks;

namespace SportsBetting.Data.Repositories.Interfaces
{
    public interface IResultRepository
    {
        Task CreateAsync(Result result);
        Task SaveChangesAsync();
        Task<Result?> GetById(int id);
    }
}