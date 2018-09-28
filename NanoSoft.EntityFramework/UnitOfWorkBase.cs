using NanoSoft.Repository;
using System;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public abstract class UnitOfWorkBase : IDefaultUnitOfWork
    {
        private readonly NanoSoftDbContext _context;

        protected UnitOfWorkBase(NanoSoftDbContext context)
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
