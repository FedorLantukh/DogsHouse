using Domain.Models;

namespace AppLogic.Services
{
    public interface IDogService
    {
        Task CreateDogAsync(Dog dog);
        Task<IEnumerable<Dog>> GetAllDogsAsync();
        Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, string order, int pageNumber = 1, int limit = 10);
    }
}