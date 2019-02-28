using NanoSoft.Extensions;
using NanoSoft.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NanoSoft.AspNetCore
{
    public class HttpIdentityService : IIdentityService
    {
        public HttpIdentityService(HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Secret", "ksnfi480r328ry932bf9ua");
        }

        public ModelState ModelState { get; } = new ModelState();
        public HttpClient Client { get; }
        public const string BaseUri = "http://localhost:4000/api";


        public async Task<Response<List<KeyValuePair>>> GetAllAsync()
        {
            var result = await Client.GetAsync($"{BaseUri}/identities");

            var content = await result.Content.ReadAsStringAsync();

            return Response.Success(content.Deserialize<List<KeyValuePair>>());
        }

        public async Task<Response<BaseIdentityUser>> FindAsync(Guid id)
        {
            var message = await Client.GetAsync($"{BaseUri}/identities/{id}");

            var response = await HttpResponse<IdentityUser>.GetResponseAsync(message);
            return response.ToOtherModel(i => (BaseIdentityUser)i);
        }


        public async Task<Response<BaseIdentityUser>> UpdateIdentityAsync(Guid id, InputModel model)
        {
            if (!ModelState.IsValid(model))
                return ModelState.GetResponse();

            var send = new StringContent(model.Serialize(), Encoding.UTF8, "application/json");
            var message = await Client.PutAsync($"{BaseUri}/identities/{id}", send);

            var response = await HttpResponse<IdentityUser>.GetResponseAsync(message);
            return response.ToOtherModel(i => (BaseIdentityUser)i);
        }

        public async Task<Response<BaseIdentityUser>> CreateIdentityAsync(InputModel model)
        {
            if (!ModelState.IsValid(model))
                return ModelState.GetResponse();

            var send = new StringContent(model.Serialize(), Encoding.UTF8, "application/json");
            var message = await Client.PostAsync($"{BaseUri}/identities/", send);

            var response = await HttpResponse<IdentityUser>.GetResponseAsync(message);
            return response.ToOtherModel(i => (BaseIdentityUser)i);
        }

        public void Dispose()
        {

        }

        public async Task<Response> DeleteIdentityAsync(Guid id)
        {
            var result = await Client.DeleteAsync($"{BaseUri}/identities/{id}");

            if (!result.IsSuccessStatusCode)
                return Response.Fail(result.ReasonPhrase);

            return Response.SuccessDelete();
        }
    }
}
