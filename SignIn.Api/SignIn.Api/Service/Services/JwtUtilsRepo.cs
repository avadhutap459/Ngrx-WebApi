using Microsoft.IdentityModel.Tokens;
using SignIn.Api.Database.DbModel;
using SignIn.Api.Database.EF;
using SignIn.Api.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SignIn.Api.Service.Services
{
  public class JwtUtilsRepo : GenericRepo<string> , IJwtUtilsRepo
  {
    private readonly AppDbContext _appDbContext;

    public JwtUtilsRepo(AppDbContext appDbContext) : base(appDbContext)
    {
      _appDbContext = appDbContext;
    }

    ~JwtUtilsRepo()
    {
      Dispose(false);
    }

    public JwtSecurityToken CreateToken(List<Claim> authClaims, string Secret, int tokenValidityInMinutes, string ValidIssuer, string ValidAudience)
    {
      try
      {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

        var token = new JwtSecurityToken(
            issuer: ValidIssuer,
            audience: ValidAudience,
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
      }
      catch (Exception ex)
      {
        throw;
      }

    }

    public string GenerateRefreshToken()
    {
      try
      {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
      catch (Exception ex)
      {
        throw;
      }

    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token, string Secret)
    {
      try
      {
        var tokenValidationParameters = new TokenValidationParameters
        {
          ValidateAudience = false,
          ValidateIssuer = false,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
          ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
          throw new SecurityTokenException("Invalid token");

        return principal;

      }
      catch (Exception ex)
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
        if(disposing)
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
