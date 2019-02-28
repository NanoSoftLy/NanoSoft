
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NanoSoft.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<Response<T>> GetAsync<T>(this HttpClient httpClient, string uri, object query = null)
        {
            uri = query == null
                ? uri
                : $"{uri}?{GetQueryString(query)}";

            HttpResponseMessage message;

            try
            {
                message = await httpClient.GetAsync(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }

        public static async Task<Response<T>> PostAsync<T>(this HttpClient httpClient, string uri, [NotNull] object obj)
        {
            var input = obj.Serialize();

            HttpResponseMessage message;

            try
            {
                message = await httpClient.PostAsync(uri, new StringContent(input, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }

        public static async Task<Response<T>> PutAsync<T>(this HttpClient httpClient, string uri, [NotNull] object obj)
        {
            var input = obj.Serialize();

            HttpResponseMessage message;

            try
            {
                message = await httpClient.PutAsync(uri, new StringContent(input, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }

        public static async Task<Response<T>> DeleteAsync<T>(this HttpClient httpClient, string uri)
        {
            HttpResponseMessage message;

            try
            {
                message = await httpClient.DeleteAsync(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }

        public static async Task<Response<T>> PatchAsync<T>(this HttpClient httpClient, string uri, object input = null)
        {
            HttpResponseMessage message;

            try
            {
                message = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), uri)
                {
                    Content = input == null
                        ? null
                        : new StringContent(input.Serialize(), Encoding.UTF8, "application/json")
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }


        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select FirstCharacterToLower(p.Name) + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        private static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
