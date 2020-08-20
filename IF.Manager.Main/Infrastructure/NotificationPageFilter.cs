using IF.Web.Mvc.Notification;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IF.Manager.Main.Infrastructure
{
    public class NotificationPageFilterAsync : IAsyncPageFilter, IPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            PageHandlerExecutedContext resultContext;


            resultContext = await next();

            if ((resultContext.HandlerInstance is PageModel))
            {

                var result = (PageModel)resultContext.HandlerInstance;

                foreach (var messageType in System.Enum.GetNames(typeof(MessageType)))
                {
                    var message = String.Empty;

                    if (result.ViewData.ContainsKey(messageType))
                    {
                        message = result.ViewData[messageType].ToString();
                    }
                    else if (result.TempData.ContainsKey(messageType))
                    {
                        message = result.TempData[messageType].ToString();
                    }


                    if (!String.IsNullOrWhiteSpace(message))
                    {

                        var jsonResult = new JsonResultModel();
                        jsonResult.AddMessage(message);
                        var msg = JsonConvert.SerializeObject(jsonResult);
                        context.HttpContext.Response.Headers.Add("X-Message-Type", messageType);
                        context.HttpContext.Response.Headers.Add("X-Message", msg);
                    }
                }
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (!(context.Result is PageResult))
                return;

            var result = (PageResult)context.Result;


            foreach (var messageType in System.Enum.GetNames(typeof(MessageType)))
            {
                var message = String.Empty;

                if (result.ViewData.ContainsKey(messageType))
                {
                    message = result.ViewData[messageType].ToString();
                }
                else if (result.Page != null && result.Page.TempData.ContainsKey(messageType))
                {
                    message = result.Page.TempData[messageType].ToString();
                }


                if (!String.IsNullOrWhiteSpace(message))
                {
                    var jsonResult = new JsonResultModel();
                    jsonResult.AddMessage(message.ToString());
                    context.HttpContext.Response.Headers.Add("X-Message-Type", messageType);
                    context.HttpContext.Response.Headers.Add("X-Message", JsonConvert.SerializeObject(jsonResult));
                    return;
                }
            }
        }



        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;


        }
    }

    

    public static class HtmlHelperExtensions
    {
        public static HtmlString RenderMessages(this IHtmlHelper htmlHelper)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var messageType in System.Enum.GetNames(typeof(MessageType)))
            {
                var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType)
                                ? htmlHelper.ViewContext.ViewData[messageType].ToString()
                                : htmlHelper.ViewContext.TempData.ContainsKey(messageType)
                                    ? htmlHelper.ViewContext.TempData[messageType].ToString()
                                    : null;
                if (!String.IsNullOrWhiteSpace(message))
                {
                    sb.AppendLine("<script language='javascript'>");
                    var result = new JsonResultModel();
                    result.AddMessage(message);
                    var messageContext = new { Type = messageType, Context = result };
                    sb.AppendLine("var messageContext =" + JsonConvert.SerializeObject(messageContext));
                    sb.AppendLine("</script>");
                }
            }
            HtmlString htmlString = new HtmlString(sb.ToString());
            return htmlString;
        }
    }

}
