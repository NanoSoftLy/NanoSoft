using NanoSoft.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NanoSoft
{
    public class HttpResponse<TModel>
    {
        public static async Task<HttpResponse<TModel>> GetResponseAsync(HttpResponseMessage message)
        {
            var contentString = await message.Content.ReadAsStringAsync();

            Console.WriteLine(contentString);

            var response = contentString.DeserializeOrDefault<HttpResponse<TModel>>();

            Console.WriteLine(response);

            switch (message.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    response.State = ResponseState.Valid;
                    break;

                case HttpStatusCode.BadRequest:
                    response.State = ResponseState.BadRequest;
                    break;

                case HttpStatusCode.Forbidden:
                    return new HttpResponse<TModel>() { State = ResponseState.Forbidden };

                case HttpStatusCode.Unauthorized:
                    return new HttpResponse<TModel>() { State = ResponseState.Unauthorized };

                case HttpStatusCode.NotAcceptable:
                    response.State = ResponseState.Unacceptable;
                    break;

                case HttpStatusCode.NotFound:
                    response.State = ResponseState.NotFound;
                    break;
                default:
                    throw new HttpRequestException(message.ReasonPhrase);
            }


            return response;
        }

        public static implicit operator Response<TModel>(HttpResponse<TModel> httpResponse)
        {
            if (httpResponse.State == ResponseState.Valid)
                return Response.Success(httpResponse.Result, httpResponse.Message);

            return Response.Fail(httpResponse.State, httpResponse.Message, httpResponse.Errors);
        }


        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

        public string Message { get; set; }

        public ResponseState State { get; set; }

        public TModel Result { get; set; }

        public HttpResponse<TOther> ToOtherModel<TOther>(Func<TModel, TOther> func)
        {
            return new HttpResponse<TOther>()
            {
                Errors = Errors,
                Message = Message,
                Result = func(Result),
                State = State
            };
        }
    }
}
