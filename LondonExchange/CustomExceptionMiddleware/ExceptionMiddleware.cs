using LondonExchange.Controllers;
using LondonExchange.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LondonExchange.CustomExceptionMiddleware
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<LondonExchangeTransactionsController> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<LondonExchangeTransactionsController> logger)
    {
      _logger = logger;
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (AccessViolationException avEx)
      {
        _logger.LogError($"A new violation exception has been thrown: {avEx}");
        await HandleExceptionAsync(httpContext, avEx);
      }
      catch (DbUpdateException dbUpdateEx)
      {
        _logger.LogError($"An error occurred while saving the entity changes: {dbUpdateEx}");
        await HandleExceptionAsync(httpContext, dbUpdateEx);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Something went wrong: {ex}");
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      var message = exception switch
      {
        AccessViolationException => "Access violation error.",
        DbUpdateException => "Database update error while saving the entity changes.",
        _ => "Internal Server Error from the custom middleware."
      };
      await context.Response.WriteAsync(new DetailedError()
      {
        StatusCode = context.Response.StatusCode,
        Message = message
      }.ToString());
    }
  }
}
