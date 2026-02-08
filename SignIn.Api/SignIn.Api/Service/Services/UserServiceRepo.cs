using Microsoft.EntityFrameworkCore;
using SignIn.Api.Database.DbModel;
using SignIn.Api.Database.EF;
using SignIn.Api.Service.Interface;

namespace SignIn.Api.Service.Services
{
  public class UserServiceRepo : GenericRepo<ClsUser>, IUserServiceRepo
  {
    private readonly AppDbContext _appDbContext;

    public UserServiceRepo(AppDbContext appDbContext) : base(appDbContext)
    {
      _appDbContext = appDbContext;
    }

    ~UserServiceRepo()
    {
      Dispose(false);
    }

    public bool? ChkValidCrendential(string Email, string password)
    {
      try
      {
        return _appDbContext.Users.Any(x=>x.EmailId == Email && x.Password == password);
      }
      catch(Exception ex)
      {
        throw;
      }
    }

    public void UpdateRefreshToken(string EmailId, string RefreshToken, DateTime RefreshTokenExpireTime)
    {
      try
      {
        var userObj = _appDbContext.Users.FirstOrDefault(x => x.EmailId == EmailId);

        if(userObj != null)
        {
          userObj.RefreshToken = RefreshToken;
          userObj.RefreshTokenExpiryTime = RefreshTokenExpireTime;
        }

        _appDbContext.Entry(userObj).State = EntityState.Modified;
        _appDbContext.SaveChanges();

      }
      catch(Exception ex)
      {
        throw;
      }
    }

    public ClsUser GetUserByEmailId(string emailId)
    {
      try
      {
        return _appDbContext.Users.FirstOrDefault(x => x.EmailId == emailId);
      }
      catch(Exception ex)
      {
        throw;
      }
    }


    #region IDisposable Support

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {

        }
        else
        {

        }

        disposedValue = true;
      }
      else
      {

      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
