//using Microsoft.AspNetCore.Antiforgery;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IF.Manager.Model
//{
//    public static class HttpContextExtentions

//    {

//        public static string GetAntiforgeryToken(this HttpContext httpContext)
//        {
//            var antiforgery = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
//            var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
//            //string fieldName = tokenSet.FormFieldName;
//            string requestToken = tokenSet.RequestToken;
//            return requestToken;
//        }
//    }
//}
