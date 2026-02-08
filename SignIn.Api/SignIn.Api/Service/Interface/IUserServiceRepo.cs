using SignIn.Api.Database.DbModel;

namespace SignIn.Api.Service.Interface
{
  public interface IUserServiceRepo : IGenericRepo<ClsUser>
  {
    bool? ChkValidCrendential(string Email, string password);
    void UpdateRefreshToken(string EmailId, string RefreshToken, DateTime RefreshTokenExpireTime);
    ClsUser GetUserByEmailId(string emailId);
  }
}
