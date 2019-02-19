using NanoSoft.Repository;
using System;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public abstract class UnitOfWorkBase : IDefaultUnitOfWork
    {
        private readonly INanoSoftDbContext _context;

        protected UnitOfWorkBase(INanoSoftDbContext context)
        {
            _context = context;
        }

        public EntityValidationState ValidationState { get; private set; } = EntityValidationState.Valid;

        public async Task CompleteAsync()
        {
            ValidationState = await _context.ValidatableSaveChangesAsync();

            if (ValidationState.IsValid)
                return;

            throw new Exception(ValidationState.Message);
        }

        public async Task<bool> TryCompleteAsync()
        {
            try
            {
                ValidationState = await _context.ValidatableSaveChangesAsync();
            }
            catch (Exception e)
            {
                ValidationState = EntityValidationState.Error(null, e.ToString());
                return false;
            }

            return ValidationState.IsValid;
        }

        public Task CompleteAsync(bool enableValidation)
        {
            if (enableValidation)
                return CompleteAsync();

            return _context.SaveChangesAsync();
        }

        public async Task<bool> TryCompleteAsync(bool enableValidation)
        {
            if (enableValidation)
                return await TryCompleteAsync();

            await _context.SaveChangesAsync();
            return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
