using System.Net;
using Api.Controllers.DTO.ResponseModels;
using Dal.Exceptions;

namespace Api.MIddlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                HttpStatusCode code = DetectStatusCode(e);

                context.Response.StatusCode = (int)code;
                var responseModel = new DefaultErrorResponseModel { Detail = e.Message };
                await context.Response.WriteAsJsonAsync(responseModel);
            }
        }

        private HttpStatusCode DetectStatusCode(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException
                    or FileNotFoundException
                    or NotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                BadHttpRequestException
                    or ArgumentException
                    or InvalidOperationException
                    or ObjectAlreadyExistsException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}

