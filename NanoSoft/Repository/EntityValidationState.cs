using JetBrains.Annotations;
using NanoSoft.Extensions;
using System;
using System.Linq.Expressions;

namespace NanoSoft.Repository
{
    [PublicAPI]
    public class EntityValidationState
    {
        public static EntityValidationState Valid => new EntityValidationState()
        {
            IsValid = true
        };

        [MustUseReturnValue]
        public static EntityValidationState Error(Expression<Func<object, object>> expression, string message)
            => new EntityValidationState()
            {
                IsValid = false,
                Message = message,
                PropertyName = expression?.ToExpressionTarget()
            };

        private EntityValidationState()
        {

        }

        public string Message { get; private set; }
        public bool IsValid { get; private set; }
        public string PropertyName { get; private set; }
    }
}