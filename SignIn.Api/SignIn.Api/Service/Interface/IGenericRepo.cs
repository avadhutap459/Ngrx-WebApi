namespace SignIn.Api.Service.Interface
{
  public interface IGenericRepo<T> where T : class
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(object Id);
    Task InsertAsync(T Entity);
    Task UpdateAsync(T Entity);
    Task DeleteAsync(object Id);
    Task SaveAsync();
  }
}
