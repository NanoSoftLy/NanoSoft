using System.Collections.Generic;

namespace NanoSoft
{
    public class ServiceProvider
    {
        private readonly List<object> _services;

        public ServiceProvider()
        {
            _services = new List<object>();
        }

        public ServiceProvider(List<object> services)
        {
            _services = services;
        }

        public void Add(object service) => _services.Add(service);
        public TService Get<TService>() => (TService)_services.Find(s => s.GetType() == typeof(TService));
    }
}
