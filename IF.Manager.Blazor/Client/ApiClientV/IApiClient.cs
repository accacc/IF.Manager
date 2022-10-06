//using IF.Core.Data;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace BlazorApp2.Client.ApiHttpClient
//{
//    public interface IApiClient
//    {
//        Task<T> PostAsync<T>(string urlPath, BaseCommand @object, Dictionary<string, string> requestHeaders = null, CancellationToken cancellationToken = default) where T : BaseCommand;
//        Task<T> PutAsync<T>(string urlPath, BaseCommand @object, Dictionary<string, string> requestHeaders = null, CancellationToken cancellationToken = default) where T : BaseCommand;
//        Task<T> DeleteAsync<T>(string urlPath, BaseCommand @object, Dictionary<string, string> requestHeaders = null, CancellationToken cancellationToken = default) where T : BaseCommand;

//        Task<T> GetAsync<T>(string urlPath, BaseRequest @params, Dictionary<string, string> requestHeaders = null, CancellationToken cancellationToken = default) where T : BaseResponse;
//    }
//}
