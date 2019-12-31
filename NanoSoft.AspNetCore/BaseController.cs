using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NanoSoft.Extensions;
using NanoSoft.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace NanoSoft.AspNetCore
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected virtual TModel LoadModelWithFiles<TModel>(IEnumerable<IFormFile> files, Func<TModel, ICollection<File>> targetProperty)
        {
            Request.Form.TryGetValue("", out var values);

            var model = values.ToString().Deserialize<TModel>();
            var property = targetProperty(model);

            foreach (var file in files)
            {
                property.Add(new File()
                {
                    Path = file.FileName,
                    Stream = file.OpenReadStream()
                });
            }

            return model;
        }


        protected virtual TModel LoadModelWithFile<TModel>(IFormFile file, Expression<Func<TModel, File>> target)
        {
            Request.Form.TryGetValue("", out var values);

            var model = values.ToString().Deserialize<TModel>();

            model.SetValue(target, new File()
            {
                Path = file.FileName,
                Stream = file.OpenReadStream()
            });

            return model;
        }

        protected virtual IActionResult GetResult(Response response, bool createdResultIfValid = false)
                => GetResult<object>(response, createdResultIfValid).Result;

        protected virtual ActionResult<TModel> GetResult<TModel>(Response<TModel> response, bool createdResultIfValid = false)
        {
            switch (response.State)
            {
                case ResponseState.Valid:
                    if (response.Model == null)
                        return createdResultIfValid
                            ? (ActionResult)Created("/", new ValidResult(response))
                            : Ok(new ValidResult(response));

                    return createdResultIfValid
                        ? Created("/", new ValidResult<TModel>(response))
                        : (ActionResult<TModel>)Ok(new ValidResult<TModel>(response));

                case ResponseState.BadRequest:
                    return response.Errors.Any()
                        ? BadRequest(new ErrorsResult(response))
                        : BadRequest(new MessageResult(response));

                case ResponseState.NotFound:
                    return NotFound(new MessageResult(response));

                case ResponseState.Forbidden:
                    return Forbid();

                case ResponseState.Unauthorized:
                    return Unauthorized(new MessageResult(response));

                case ResponseState.Unacceptable:
                    return response.Errors.Any()
                        ? StatusCode((int)HttpStatusCode.NotAcceptable, new ErrorsResult(response))
                        : StatusCode((int)HttpStatusCode.NotAcceptable, new MessageResult(response));

                default:
                    throw new ArgumentOutOfRangeException(nameof(response.State));
            }
        }


        public class ValidResult
        {
            public ValidResult(Response response)
            {
                Message = response.Message;
            }

            public string Message { get; set; }
        }

        public class ValidResult<T> : ValidResult
        {
            public ValidResult(Response<T> response) : base(response)
            {
                Result = response.Model;
            }

            public T Result { get; set; }
        }

        public class ErrorsResult
        {
            public ErrorsResult(Response response)
            {
                Errors = response.Errors;
            }

            public Dictionary<string, List<string>> Errors { get; set; }
        }

        public class MessageResult
        {
            public MessageResult(Response response)
            {
                Message = response.Message;
            }

            public string Message { get; set; }
        }
    }
}
