﻿using JetBrains.Annotations;
using NanoSoft.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NanoSoft
{
    public interface IValidator
    {
        bool IsValid(object model);
        Response AddError([NotNull] Expression<Func<object, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest);
        Response AddError(string name, string message, ResponseState state = ResponseState.BadRequest);
        Response AddError(EntityValidationState entityValidationState, ResponseState state = ResponseState.BadRequest);
        Response<TModel> AddError<TModel>([NotNull] Expression<Func<TModel, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest);
        Response<TModel> AddError<TModel>([NotNull] Expression<Func<object, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest);
        Response<TModel> AddError<TModel>(string name, string message, ResponseState state = ResponseState.BadRequest);
        Response<TModel> AddError<TModel>(EntityValidationState entityValidationState, ResponseState state = ResponseState.BadRequest);
        Dictionary<string, List<string>> Errors { get; }
        void Clear();
    }
}