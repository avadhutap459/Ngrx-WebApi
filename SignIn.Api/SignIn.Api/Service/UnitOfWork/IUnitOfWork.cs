using SignIn.Api.Service.Interface;
using SignIn.Api.Service.Services;

namespace SignIn.Api.Service.UnitOfWork
{
  public interface IUnitOfWork 
  {
    //Define the Specific Repositories
    JwtUtilsRepo JwtUtils { get; }
    UserServiceRepo UserService { get; }
    //This Method will Start the database Transaction
    void CreateTransaction();
    //This Method will Commit the database Transaction
    void Commit();
    //This Method will Rollback the database Transaction
    void Rollback();
    //This Method will call the SaveChanges method
    Task Save();
  }
}
