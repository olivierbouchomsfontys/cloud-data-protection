using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CloudDataProtection.Core.Rest.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CloudDataProtection.Core.Http.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                HttpResponse response = context.Response;
                
                response.ContentType = "application/json";

                object body;

                switch(error)
                {
                    case KeyNotFoundException _:
                        // not found error
                        body = NotFoundResponse.RouteIncorrect();
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        body = InternalServerErrorResponse.Create();
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await response.WriteAsync(JsonConvert.SerializeObject(body));
            }
        }
    }
}