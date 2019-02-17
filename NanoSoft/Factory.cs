using Bogus;
using JetBrains.Annotations;
using NanoSoft.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace NanoSoft
{
    [PublicAPI]
    public abstract class Factory<TUnitOfWork, TObject>
        where TUnitOfWork : IDefaultUnitOfWork
        where TObject : class
    {
        protected readonly Faker Faker = new Faker();
        protected TUnitOfWork UnitOfWork { get; }

        protected Factory(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public TObject Make()
        {
            try
            {
                var obj = ObjectFactory.Create<TObject>();

                foreach (var property in GetType().GetRuntimeProperties())
                {
                    obj.SetValue(property.Name, property.GetValue(this));
                }

                return obj;
            }
            catch (Exception e)
            {
                throw new FactoryMakerException(e.ToString(), e);
            }
        }

        public List<TObject> MakeRange(int number)
        {
            var list = new List<TObject>();
            for (var i = 0; i < number; i++)
                list.Add(Make());

            return list;
        }

        protected abstract Task SaveAsync(TObject obj);
        protected abstract Task SaveRangeAsync(IEnumerable<TObject> objects);

        public async Task<TObject> CreateAsync()
        {
            TObject obj;
            using (UnitOfWork)
            {
                obj = Make();
                await SaveAsync(obj);
                await CompleteAsync();
            }
            return obj;
        }

        public async Task<List<TObject>> CreateRangeAsync(int number)
        {
            var list = MakeRange(number);
            using (UnitOfWork)
            {
                await SaveRangeAsync(list);
                await CompleteAsync();
            }
            return list;
        }


        protected virtual Task CompleteAsync()
        {
            return UnitOfWork.CompleteAsync();
        }
    }
}
