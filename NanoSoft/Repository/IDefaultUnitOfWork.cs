using JetBrains.Annotations;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.Repository
{
    [PublicAPI]
    public interface IDefaultUnitOfWork : IDisposable
    {
        [NotNull]
        EntityValidationState ValidationState { get; }

        Task CompleteAsync();

        Task<bool> TryCompleteAsync();
    }

    [PublicAPI]
    public interface IDefaultUnitOfWork<TNotificationTypes> : IDefaultUnitOfWork
    {
        Task CompleteAsync([CanBeNull] Expression<Func<TNotificationTypes, bool>> expression);
        Task<bool> TryCompleteAsync([CanBeNull] Expression<Func<TNotificationTypes, bool>> expression);
    }
}
