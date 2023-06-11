using DataAccess.Context;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : class, IEntity, new()
    {
        protected readonly DogDbContext _context;
        protected readonly DbSet<Entity> _entities;

        public Repository(DogDbContext context)
        {
            _context = context;
            _entities = context.Set<Entity>();
        }
        public async Task AddEntityAsync(Entity entity)
        {
            _entities.Add(entity);
        }

        public async Task<IEnumerable<Entity>> GetAllEntitiesAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task RepoSaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
