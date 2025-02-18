using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using HXQ.NWBC_Assignment05_v50.Data.Models;

namespace HXQ.NWBC_Assignment05_v50.Data.Exceptions
{
    //này là tùy chỉnh exceptionmiddleware tích hợp, custom ở lớp kia
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureBuildInExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorVm()
                        {
                            ErrorCode = context.Response.StatusCode,
                            ErrorMessage = contextFeature.Error.Message,
                            Path = contextRequest.Path
                        }.ToString());
                    }
                });
            });
        }
    }
}
