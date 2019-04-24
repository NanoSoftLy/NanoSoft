using JetBrains.Annotations;
using System;
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

        Task CompleteAsync(bool enableValidation);

        Task<bool> TryCompleteAsync(bool enableValidation);
    }
}
