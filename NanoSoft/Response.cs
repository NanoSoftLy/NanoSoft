using NanoSoft.Resources;
using System.Collections.Generic;

namespace NanoSoft
{
    public struct Response
    {
        public static Response Success(string message) => new Response(ResponseState.Valid, message);

        public static Response SuccessCreate() => new Response(ResponseState.Valid, SharedMessages.SuccessCreate);

        public static Response SuccessEdit() => new Response(ResponseState.Valid, SharedMessages.SuccessEdit);

        public static Response SuccessDelete() => new Response(ResponseState.Valid, SharedMessages.SuccessDelete);

        public static Response Fail(string message) => new Response(ResponseState.BadRequest, message);

        public static Response Fail(ResponseState state) => new Response(state, null);

        public static Response Fail(ResponseState state, string message) => new Response(state, message);

        public static Response Fail(IValidator validator, ResponseState state = ResponseState.BadRequest)
            => new Response(validator.Errors, state);

        private Response(ResponseState state, string message)
        {
            Errors = new Dictionary<string, List<string>>();
            Message = message;
            _state = state;
        }

        private Response(Dictionary<string, List<string>> errors, ResponseState state)
        {
            Errors = errors;
            Message = null;
            _state = state;
        }

        public string Message { get; private set; }
        public bool IsValid => State == ResponseState.Valid;
        private ResponseState _state;
        public ResponseState State
        {
            get => _state == ResponseState.Valid && Errors.Keys.Count > 0
                    ? ResponseState.BadRequest
                    : _state;
            private set => _state = value;
        }
        public Dictionary<string, List<string>> Errors { get; private set; }
    }

    public struct Response<TModel>
    {
        public static Response<TModel> Success(TModel model) => new Response<TModel>(model, Response.Success(null));
        public static Response<TModel> Success(TModel model, string message) => new Response<TModel>(model, Response.Success(message));

        public static Response<TModel> SuccessCreate(TModel model) => new Response<TModel>(model, Response.SuccessCreate());

        public static Response<TModel> SuccessEdit(TModel model) => new Response<TModel>(model, Response.SuccessEdit());

        public static Response<TModel> SuccessDelete(TModel model)
            => new Response<TModel>(model, Response.SuccessDelete());

        public static Response<TModel> Fail(string message)
            => new Response<TModel>(default(TModel), Response.Fail(message));

        public static Response<TModel> Fail(ResponseState state)
            => new Response<TModel>(default(TModel), Response.Fail(state));

        public static Response<TModel> Fail(ResponseState state, string message)
            => new Response<TModel>(default(TModel), Response.Fail(state, message));

        public static Response<TModel> Fail(IValidator validator, ResponseState state = ResponseState.BadRequest)
            => new Response<TModel>(default(TModel), Response.Fail(validator, state));


        public Response(Response response)
        {
            InnerResponse = response;
            Model = default(TModel);
        }

        public Response(TModel model, Response response)
        {
            InnerResponse = response;
            Model = model;
        }

        private Response InnerResponse { get; }
        public TModel Model { get; private set; }
        public string Message => InnerResponse.Message;
        public bool IsValid => InnerResponse.IsValid;
        public ResponseState State => InnerResponse.State;
        public Dictionary<string, List<string>> Errors => InnerResponse.Errors;
    }
}
