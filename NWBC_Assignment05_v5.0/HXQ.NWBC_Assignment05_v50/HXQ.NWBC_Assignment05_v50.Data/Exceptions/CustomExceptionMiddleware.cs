using HXQ.NWBC_Assignment05_v50.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace HXQ.NWBC_Assignment05_v50.Data.Exceptions
{
    //exception middleware custom
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Chạy tiếp middleware tiếp theo
                await _next(context); 
            }
            catch (Exception e)
            {
                _logger.LogError($"Có lỗi xảy ra: {e}");

                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorVm { Path = context.Request.Path };

                switch (e)
                {
                    //catch lỗi quyền truy cập
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.ErrorCode = response.StatusCode;
                        errorResponse.ErrorMessage = "Không có quyền truy cập!";
                        break;

                    //catch lỗi không tìm thấy dữ liệu
                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.ErrorCode = response.StatusCode;
                        errorResponse.ErrorMessage = "Không tìm thấy dữ liệu!";
                        break;
                    
                    //catch lỗi xác minh dữ liệu
                    case ValidationException validationEx:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.ErrorCode = response.StatusCode;
                        errorResponse.ErrorMessage = validationEx.Message;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.ErrorCode = response.StatusCode;
                        errorResponse.ErrorMessage = "Lỗi hệ thống, vui lòng thử lại sau!";
                        break;
                }

                var errorJson = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(errorJson);
            }
        }
    }
}
