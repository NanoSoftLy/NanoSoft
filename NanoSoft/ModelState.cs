using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace NanoSoft
{
    public class ModelState : IValidator
    {
        public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

        public Response AddError([NotNull] Expression<Func<object, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest) => AddError(expression.ToExpressionTarget(), message, state);

        public Response AddError(string name, string message, ResponseState state = ResponseState.BadRequest)
        {
            TryAddError(name, message);

            return Response.Fail(this, state);
        }

        public Response AddError(EntityValidationState entityValidationState, ResponseState state = ResponseState.Unacceptable)
            => AddError(entityValidationState.PropertyName, entityValidationState.Message, state);


        public Response<TModel> AddError<TModel>([NotNull] Expression<Func<TModel, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest) => AddError<TModel>(expression.ToExpressionTarget(), message, state);

        public Response<TModel> AddError<TModel>([NotNull] Expression<Func<object, object>> expression, [NotNull] string message, ResponseState state = ResponseState.BadRequest) => AddError<TModel>(expression.ToExpressionTarget(), message, state);

        public Response<TModel> AddError<TModel>(string name, string message, ResponseState state = ResponseState.BadRequest)
        {
            TryAddError(name, message);
            return Response.Fail<TModel>(this, state);
        }

        public Response<TModel> AddError<TModel>(EntityValidationState entityValidationState, ResponseState state = ResponseState.Unacceptable)
            => AddError<TModel>(entityValidationState.PropertyName, entityValidationState.Message, state);


        public void Clear() => Errors.Clear();

        public bool IsValid(object model)
        {
            var context = new ValidationContext(model);

            ValidateProperties(model, context);

            if (model is IValidatable validatable)
                validatable.Validate(this);

            return !Errors.Any();
        }

        private void ValidateProperties(object model, ValidationContext context, string prefixName = null)
        {
            foreach (var property in model.GetType().GetProperties())
            {
                var results = new List<ValidationResult>();
                var value = property.GetValue(model);
                context.MemberName = property.Name;
                var keyName = (prefixName + "." + property.Name).Trim('.');

                if (!Validator.TryValidateProperty(value, context, results))
                {
                    var errors = results.Select(r => r.ErrorMessage);
                    if (Errors.ContainsKey(keyName))
                    {
                        Errors[keyName].AddRange(errors);
                        continue;
                    }

                    Errors.Add(keyName, errors.ToList());
                }

                if (property.PropertyType.Namespace != "System"
                    && value != null
                    && value is IEnumerable enumerale)
                {
                    var index = 0;
                    foreach (var item in enumerale)
                    {
                        ValidateProperties(item, new ValidationContext(item), $"{property.Name}[{index}]");
                        index++;
                    }
                }
            }
        }

        private void TryAddError(string name, string message)
        {
            if (Errors.ContainsKey(name))
                Errors[name].Add(message);
            else
                Errors.Add(name, new List<string>() { message });
        }
    }
}
