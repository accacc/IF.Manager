using IF.Core.Log;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IF.Manager.Main.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }          

            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            //TODO:logla
            //var logger = (ILogService)context.RequestServices.GetService(typeof(ILogService));

            //logger.Error(ex, "web_ui", ex.Message, "1", Guid.NewGuid(), context.Connection.RemoteIpAddress.MapToIPv4().ToString(), "web");

            await next(context);

        }
    }

}
