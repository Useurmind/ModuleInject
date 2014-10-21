using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    public class DependencyContainer
    {
        private IList<IContainerRegistration> registrations;

        public IEnumerable<IContainerRegistration> Registrations
        {
            get
            {
                return registrations;
            }
        }

        public DependencyContainer()
        {
            registrations = new List<IContainerRegistration>();
        }

        public void Register<T>()
        {
            registrations.Add(new TypeRegistration()
                                  {
                                      ActualType = typeof(T)
                                  });
        }


    }
}
