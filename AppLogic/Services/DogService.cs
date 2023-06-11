using DataAccess.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AppLogic.Services
{
    public class DogService : IDogService
    {
        private readonly IRepository<Dog> _dogRepo;

        public DogService(IRepository<Dog> dogRepo)
        {
            _dogRepo = dogRepo;
        }

        public async Task<IEnumerable<Dog>> GetAllDogsAsync()
        {
            return (await _dogRepo.GetAllEntitiesAsync());
        }

        public async Task CreateDogAsync(Dog dog)
        {
            await _dogRepo.AddEntityAsync(dog);
            await _dogRepo.RepoSaveChangesAsync();

        }
        public async Task<IEnumerable<Dog>> GetSortedDogsAsync(string attribute, string order, int pageNumber = 1, int limit = 10) 
        {
            var query = await GetAllDogsAsync();

            if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
            {
                var prop = typeof(Dog)
                    .GetProperty(attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (prop != null)
                {
                    if (order.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        query = query
                            .OrderByDescending(x => prop
                            .GetValue(x, null));
                    }
                    else
                    {
                        query = query
                            .OrderBy(x => prop
                            .GetValue(x, null));
                    }
                }
            }
            return await PaginateAsync(query.ToList(), pageNumber, limit);
        }
        private async Task<IEnumerable<Dog>> PaginateAsync(List<Dog> query, int pageNumber, int limit)
        {
            var dogs = query
                           .Skip((pageNumber - 1) * limit)
                           .Take(limit)
                           .ToList();

            return await Task.FromResult(dogs);
        }
    }
}
