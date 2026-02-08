using Microsoft.EntityFrameworkCore;
using SignIn.Api.Database.EF;
using SignIn.Api.Service.Interface;

namespace SignIn.Api.Service.Services
{
  public class GenericRepo<T> : IGenericRepo<T> where T : class
  {
    protected readonly AppDbContext appDbContext;
    protected readonly DbSet<T> _dbSet;
    public GenericRepo(AppDbContext appDbContext)
    {
      this.appDbContext = appDbContext;
      _dbSet = appDbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }
    public async Task<T?> GetByIdAsync(object Id)
    {
      return await _dbSet.FindAsync(Id);
    }
    public async Task InsertAsync(T Entity)
    {
      //It will mark the Entity state as Added
      await _dbSet.AddAsync(Entity);
    }
    public async Task UpdateAsync(T Entity)
    {
      //It will mark the Entity state as Modified
      _dbSet.Update(Entity);
    }
    public async Task DeleteAsync(object Id)
    {
      //First, fetch the record from the table
      var entity = await _dbSet.FindAsync(Id);
      if (entity != null)
      {
        //This will mark the Entity State as Deleted
        _dbSet.Remove(entity);
      }
    }
    public async Task SaveAsync()
    {
      await appDbContext.SaveChangesAsync();
    }
  }

}
