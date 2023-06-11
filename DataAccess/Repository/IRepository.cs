using Domain;

namespace DataAccess.Repository
{
    public interface IRepository<Entity> where Entity : class, IEntity, new()
    {
        Task AddEntityAsync(Entity entity);
        Task<IEnumerable<Entity>> GetAllEntitiesAsync();
        Task RepoSaveChangesAsync();
    }
}