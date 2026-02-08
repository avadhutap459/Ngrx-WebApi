using Microsoft.AspNetCore.Mvc;
using SignIn.Api.Database.DbModel;
using SignIn.Api.Models;
using SignIn.Api.Service.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SignIn.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
      _unitOfWork = unitOfWork;
      _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login(ClsUser request)
    {
      try
      {
        bool isUserExit = _unitOfWork.UserService.ChkValidCrendential(request.EmailId, request.Password).HasValue ?
          _unitOfWork.UserService.ChkValidCrendential(request.EmailId, request.Password).Value : false;

        if (isUserExit)
        {
          var authClaims = new List<Claim> { new Claim(ClaimTypes.Email, request.EmailId) };
          authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));

          _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
          string Secret = _configuration.GetValue<string>("JWT:Secret");
          _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
          string ValidAudience = _configuration.GetValue<string>("JWT:ValidAudience");
          string ValidIssuer = _configuration.GetValue<string>("JWT:ValidIssuer");

          var token = _unitOfWork.JwtUtils.CreateToken(authClaims,Secret,tokenValidityInMinutes,ValidIssuer,ValidAudience);
          string refreshToken = _unitOfWork.JwtUtils.GenerateRefreshToken();


          _unitOfWork.UserService.UpdateRefreshToken(request.EmailId,refreshToken,DateTime.Now.AddDays(refreshTokenValidityInDays));

          return Ok(new
          {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
          });
        }

        return Ok();
      }
      catch(Exception ex)
      {
        throw;
      }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(ClsTokenModel tokenModel)
    {
      string Secret = _configuration.GetValue<string>("JWT:Secret");
      _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
      _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
      string ValidAudience = _configuration.GetValue<string>("JWT:ValidAudience");
      string ValidIssuer = _configuration.GetValue<string>("JWT:ValidIssuer");

      try
      {
        if (tokenModel is null)
        {
          return BadRequest("Invalid client request");
        }

        string? accessToken = tokenModel.AccessToken;
        string? refreshToken = tokenModel.RefreshToken;

        var principal = _unitOfWork.JwtUtils.GetPrincipalFromExpiredToken(accessToken, Secret);
        if (principal == null)
        {
          return BadRequest("Invalid access token or refresh token");
        }
        string username = principal.Identity.Name;
        username = principal.Claims
    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        ClsUser objuser = _unitOfWork.UserService.GetUserByEmailId(username);
        if (objuser == null || objuser.RefreshToken != refreshToken || objuser.RefreshTokenExpiryTime <= DateTime.Now)
        {
          return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _unitOfWork.JwtUtils.CreateToken(principal.Claims.ToList(), Secret, tokenValidityInMinutes, ValidIssuer, ValidAudience);
        var newRefreshToken = _unitOfWork.JwtUtils.GenerateRefreshToken();

        _unitOfWork.UserService.UpdateRefreshToken(objuser.EmailId, refreshToken, DateTime.Now.AddDays(refreshTokenValidityInDays));

        return new ObjectResult(new
        {
          accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
          refreshToken = newRefreshToken
        });

      }
      catch(Exception ex)
      {
        throw;
      }
    }

    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
      _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
      try
      {
        ClsUser objLogin = _unitOfWork.UserService.GetUserByEmailId(username);
        if (objLogin == null) return BadRequest("Invalid user name");


        _unitOfWork.UserService.UpdateRefreshToken(username, null, DateTime.Now.AddDays(refreshTokenValidityInDays));

        return NoContent();

      }
      catch (Exception ex)
      {
        throw;
      }
    }



  }

}
