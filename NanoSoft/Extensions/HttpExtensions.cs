
using JetBrains.Annotations;
using NanoSoft.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

        public static async Task<Response<T>> PostFilesAsync<T>(this HttpClient httpClient, string uri, [NotNull] object obj, ICollection<File> files)
        {
            var multipartForm = new MultipartFormDataContent();

            foreach (var file in files)
            {
                multipartForm.Add(new StreamContent(file.Stream), "files", file.Path);
            }

            files.Clear();

            var input = obj.Serialize();

            multipartForm.Add(new StringContent(input, Encoding.UTF8, "application/json"));

            HttpResponseMessage message;

            try
            {
                message = await httpClient.PostAsync(uri, multipartForm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Response.Fail(ResponseState.Unavailable);
            }

            return await HttpResponse<T>.GetResponseAsync(message);
        }


        public static async Task<Response<T>> PutFilesAsync<T>(this HttpClient httpClient, string uri, [NotNull] object obj, ICollection<File> files)
        {
            var multipartForm = new MultipartFormDataContent();

            foreach (var file in files)
            {
                multipartForm.Add(new StreamContent(file.Stream), "files", file.Path);
            }

            files.Clear();

            var input = obj.Serialize();

            multipartForm.Add(new StringContent(input, Encoding.UTF8, "application/json"));

            HttpResponseMessage message;

            try
            {
                message = await httpClient.PutAsync(uri, multipartForm);
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
                             select UrlEncode(p, obj);

            return string.Join("&", properties.ToArray());
        }

        public static string UrlEncode(PropertyInfo p, object obj)
        {
            var name = FirstCharacterToLower(p.Name);
            var value = p.GetValue(obj, null);

            if (value is DateTime dateTime)
                return name + "=" + HttpUtility.UrlEncode(dateTime.ToDateString());

            if (value is IEnumerable collection && !(value is string))
            {
                var results = new List<string>();

                foreach (var item in collection)
                    results.Add($"{name}={item.ToString()}");

                return string.Join("&", results);
            }

            return name + "=" + HttpUtility.UrlEncode(value.ToString());
        }

        private static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    }
}
