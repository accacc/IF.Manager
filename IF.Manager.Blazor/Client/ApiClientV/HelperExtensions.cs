using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Globalization;
using System.Linq;

namespace Arbimed.Core.ApiClient
{
    public static class HelperExtensions
    {


        public static string ToHttpQuery(this object a)
        {
            string result = "";

            foreach (var b in JObject.FromObject(a))
            {
                if (result.Length > 0)
                    result += "&";

                if (b.Value.Type == JTokenType.Array)
                    result += b.Value.Select(x => b.Key + "[]=" + Uri.EscapeDataString(x.ToString())).ToList().Join("&");
                else
                    result += b.Key + "=" + Uri.EscapeDataString(b.Value.ToString());
            }

            return result;
        }

        public static string Join(this List<string> a, string seperator)
        {
            return string.Join(seperator, a.ToArray());
        }
        public static FormUrlEncodedContent ToFormData(this object obj)
        {
            var formData = obj.ToKeyValue();

            return new FormUrlEncodedContent(formData);
        }

        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            // Added by me: avoid cyclic references
            var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            if (metaToken is not JToken token)
            {
                // Modified by me: use serializer defined above
                return JObject.FromObject(metaToken, serializer).ToKeyValue();
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                 .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }
    }
}
