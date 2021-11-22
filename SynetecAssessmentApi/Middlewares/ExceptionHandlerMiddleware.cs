using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SynetecAssessmentApi.Logger;

namespace SynetecAssessmentApi.Middlewares
{
    /// <summary>
    /// Class contains the ExceptionHandler Middleware which handles all the exceptions in the application execution.
    /// </summary>
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate nextMiddleware, ILogger logger)
        {
            _nextMiddleware = nextMiddleware;
            _logger = logger;
        }

        /// <summary>
        /// ExceptionHandler Middleware invoke which will be invoked by ASP.Net Core Pipeline.
        /// </summary>
        /// <param name="context">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>Returns the task object for the next middleware.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextMiddleware(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await LogException(context, e);
            }
        }

        /// <summary>
        /// This method logs the exception.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task LogException(HttpContext context, Exception ex)
        {
            _logger.Error(ex.StackTrace);
            await context.Response.WriteAsync(ex.Message, Encoding.Default);
        }
    }
}
