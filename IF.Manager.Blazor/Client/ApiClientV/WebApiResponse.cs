using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace Arbimed.Core.ApiClient
{
    public class WebApiResponse
    {

        private HttpStatusCode statusCode;


        private HttpContent body;


        private HttpResponseHeaders headers;


        public WebApiResponse(HttpStatusCode statusCode, HttpContent responseBody, HttpResponseHeaders responseHeaders)
        {
            StatusCode = statusCode;
            Body = responseBody;
            Headers = responseHeaders;
        }


        public HttpStatusCode StatusCode
        {
            get
            {
                return statusCode;
            }

            set
            {
                statusCode = value;
            }
        }


        public HttpContent Body
        {
            get
            {
                return body;
            }

            set
            {
                body = value;
            }
        }


        public HttpResponseHeaders Headers
        {
            get
            {
                return headers;
            }

            set
            {
                headers = value;
            }
        }


        //public virtual async Task<Dictionary<string, dynamic>> DeserializeResponseBodyAsync(HttpContent content)
        //{
        //    var stringContent = await content.ReadAsStringAsync().ConfigureAwait(false);
        //    var dsContent = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(stringContent);
        //    return dsContent;
        //}


        public virtual Dictionary<string, string> DeserializeResponseHeaders(HttpResponseHeaders content)
        {
            var dsContent = new Dictionary<string, string>();
            foreach (var pair in content)
            {
                dsContent.Add(pair.Key, pair.Value.First());
            }

            return dsContent;
        }
    }
}
