using Newtonsoft.Json;
using System.Net;

namespace SignIn.Api.Global
{
  public class ExceptionHandlerMiddelware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddelware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionMessageAsync(context, ex);
      }
    }

    private async Task HandleExceptionMessageAsync(HttpContext context,Exception ex1)
    {
      try
      {
        context.Response.Clear();

        var failureObject = JsonConvert.SerializeObject(new {status = "Failed" ,statusCode = (int)HttpStatusCode.InternalServerError, Exception = ex1 });


        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var originalBody = context.Response.Body;
        await context.Response.WriteAsync(failureObject);

      }
      catch(Exception ex)
      {
        throw;
      }
    }
  }
}
