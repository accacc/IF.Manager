using IF.Core.Exception;
using IF.Web.Mvc.Exception;
using IF.Web.Mvc.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IF.Manager.Main.Infrastructure
{

    public class ExceptionPagefilterAsync : IAsyncExceptionFilter, IExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            HttpRequestExtension.HandleException(context);
            await Task.CompletedTask;
        }

        public void OnException(ExceptionContext context)
        {
            HttpRequestExtension.HandleException(context);

        }
    }

    public static class HttpRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static void HandleException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;

            JsonResultModel result = new JsonResultModel();

            if (context.Exception is BusinessException businessException)
            {
                result.AddException(context.Exception);
            }
            else if (context.Exception is ModelStateException)
            {
                ModelStateException exception = (ModelStateException)context.Exception;
                result.AddModelState(exception.ModelStateDictionary);
            }
            else if (context.Exception is ProductNotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                result.AddException(context.Exception);
            }
            else
            {
                //result.AddMessage("Bir hata oluştu");
                result.AddException(context.Exception.GetBaseException());
            }

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(result);
                //context.Result = new JsonResult(result);
                context.Result = new ContentResult() { Content = json, ContentType = "application/json", StatusCode = (int)HttpStatusCode.InternalServerError };
            }
            else
            {
                var modelMetadata = new EmptyModelMetadataProvider();
                var ViewData = new ViewDataDictionary(modelMetadata, context.ModelState);

                ViewData["HasError"] = "1";

                var errorPage = new PartialViewResult
                {
                    ViewName = "~/Pages/_CustomError.cshtml",
                    ViewData = new ViewDataDictionary<JsonResultModel>(ViewData, result)
                };

                context.Result = errorPage;
            }
        }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
