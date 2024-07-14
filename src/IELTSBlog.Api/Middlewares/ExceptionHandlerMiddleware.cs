﻿using IELTSBlog.Api.Models;
using IELTSBlog.Service.Exceptions;

namespace IELTSBlog.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        public readonly RequestDelegate _request;
        public readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate request, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _request = request;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new Response
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                });
            }
            catch (AlreadyExistException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new Response
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                });
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new Response
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                _logger.LogError(ex.ToString());

                await context.Response.WriteAsJsonAsync(new Response
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.InnerException.Message,
                });
            }
        }
    }
}
