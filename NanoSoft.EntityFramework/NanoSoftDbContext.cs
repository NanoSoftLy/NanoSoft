using Microsoft.EntityFrameworkCore;
using NanoSoft.Repository;
using System;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public abstract class NanoSoftDbContext : DbContext, INanoSoftDbContext
    {
        protected NanoSoftDbContext()
        {

        }

        protected NanoSoftDbContext(DbContextOptions options) : base(options)
        {

        }


        public Task<EntityValidationState> ValidatableSaveChangesAsync() => Task.Run(() => ValidatableSaveChanges());

        public virtual EntityValidationState ValidatableSaveChanges()
        {
            try
            {
                SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return EntityValidationState.Error(er => e.InnerException, e.ToString());
            }

            return EntityValidationState.Valid;
        }
    }
}
