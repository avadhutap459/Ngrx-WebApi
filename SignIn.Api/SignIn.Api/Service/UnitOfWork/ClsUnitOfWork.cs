using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SignIn.Api.Database.EF;
using SignIn.Api.Service.Interface;
using SignIn.Api.Service.Services;

namespace SignIn.Api.Service.UnitOfWork
{
  public class ClsUnitOfWork : IUnitOfWork, IDisposable
  {
    public AppDbContext Context = null;

    private IDbContextTransaction? _objTran = null;
    public JwtUtilsRepo JwtUtils { get; private set; }
    public UserServiceRepo UserService { get; private set; }
    public ClsUnitOfWork(AppDbContext _Context)
    {

      Context = _Context;
      JwtUtils = new JwtUtilsRepo(Context);
      UserService = new UserServiceRepo(Context);
    }

    public void CreateTransaction()
    {
      _objTran = Context.Database.BeginTransaction();
    }
    public void Commit()
    {
      _objTran?.Commit();
    }

    public void Rollback()
    {
      _objTran?.Rollback();
      _objTran?.Dispose();
    }

    public async Task Save()
    {
      try
      {
        await Context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }
    public void Dispose()
    {
      Context.Dispose();
    }
  }
}
