using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Repository;
using NanoSoft.Resources;
using System.Collections.Generic;
using System.Linq;

namespace NanoSoft
{
    [PublicAPI]
    public struct Response : IResponse
    {
        public static Response Success(string message) => new Response(ResponseState.Valid, message);

        public static Response SuccessCreate() => new Response(ResponseState.Valid, SharedMessages.SuccessCreate);

        public static Response SuccessEdit() => new Response(ResponseState.Valid, SharedMessages.SuccessEdit);

        public static Response SuccessDelete() => new Response(ResponseState.Valid, SharedMessages.SuccessDelete);

        public static Response Fail(string message) => new Response(ResponseState.BadRequest, message);

        public static Response Fail(ResponseState state) => new Response(state, null);

        public static Response Fail(ResponseState state, string message) => new Response(state, message);

        public static Response Fail(ResponseState state, string message, Dictionary<string, List<string>> errors) => new Response(state, message, errors);

        public static Response Fail(IValidator validator, ResponseState state = ResponseState.BadRequest)
            => new Response(validator.Errors, state);

        public static Response AddError(EntityValidationState entityValidationState, ResponseState state = ResponseState.Unacceptable)
            => new Response(new Dictionary<string, List<string>>()
            {
                {
                    entityValidationState.PropertyName, new List<string>(){ entityValidationState.Message }
                }
            }, state);

        public static Response<TModel> Success<TModel>(TModel model) => new Response<TModel>(model, Success(null));
        public static Response<TModel> Success<TModel>(TModel model, string message) => new Response<TModel>(model, Success(message));

        public static Response<TModel> SuccessCreate<TModel>(TModel model) => new Response<TModel>(model, SuccessCreate());

        public static Response<TModel> SuccessEdit<TModel>(TModel model) => new Response<TModel>(model, SuccessEdit());

        public static Response<TModel> SuccessDelete<TModel>(TModel model)
            => new Response<TModel>(model, SuccessDelete());


        private Response(ResponseState state, string message)
        {
            Errors = new Dictionary<string, List<string>>();
            _message = message;
            _state = state;
        }

        private Response(ResponseState state, string message, Dictionary<string, List<string>> errors)
        {
            Errors = errors;
            _message = message;
            _state = state;
        }

        private Response(Dictionary<string, List<string>> errors, ResponseState state)
        {
            Errors = errors;
            _message = null;
            _state = state;
        }

        private string _message;
        public string Message
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_message))
                    return _message;

                var errors = string.Join("\n", Errors.SelectMany(e => e.Value));

                return string.IsNullOrWhiteSpace(errors) ? _state.DisplayName() : errors;
            }
            private set => _message = value;
        }
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

    public struct Response<TModel> : IResponse
    {
        public static implicit operator Response(Response<TModel> response)
        {
            return response.InnerResponse;
        }
        public static implicit operator Response<TModel>(Response response)
        {
            return new Response<TModel>(response);
        }

        public Response(Response response)
        {
            InnerResponse = response;
            Model = default;
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
