using System;
using SimpleInjector;

namespace license_counter
{
    static class Program
    {
        static void Main(string[] args)
        {
            var injectionContainer = new Container();

            AddInjectionMappings(injectionContainer);
        }

        private static void AddInjectionMappings(Container container)
        {
            container.RegisterSingleton<>();
        }
    }
}
