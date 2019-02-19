using NanoSoft.Repository;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public interface INanoSoftDbContext : IDbContext
    {
        EntityValidationState ValidatableSaveChanges();
        Task<EntityValidationState> ValidatableSaveChangesAsync();
    }
}
