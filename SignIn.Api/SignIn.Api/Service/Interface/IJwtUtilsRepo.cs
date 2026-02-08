using SignIn.Api.Database.DbModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SignIn.Api.Service.Interface
{
  public interface IJwtUtilsRepo : IGenericRepo<string>
  {
    JwtSecurityToken CreateToken(List<Claim> authClaims, string Secret, int tokenValidityInMinutes, string ValidIssuer, string ValidAudience);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token, string Secret);
  }
}
