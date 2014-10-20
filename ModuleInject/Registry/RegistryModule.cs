using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    using System.Dynamic;

    using ModuleInject.Interfaces;
    using ModuleInject.Utility;

    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class RegistryModule : IRegistryModule
    {
        private Dictionary<Type, RegistrationEntry> _RegistrationEntries;

        public RegistryModule()
        {
            _RegistrationEntries = new Dictionary<Type, RegistrationEntry>();
        }



        internal override bool IsRegistered(Type type)
        {
            return _RegistrationEntries.ContainsKey(type);
        }

        internal override object GetComponent(Type type)
        {
            RegistrationEntry entry = null;
            if (!_RegistrationEntries.TryGetValue(type, out entry))
            {
                CommonFunctions.ThrowFormatException(Errors.RegistryModule_TypeNotRegistered, type.Name);
            }

            return entry.ResolvedInstance;
        }

        internal override IRegistryModule Merge(IRegistryModule otherRegistryModule)
        {
            RegistryModule newRegistryModule = new RegistryModule();

            try
            {

                foreach (var registrationEntry in this.GetRegistrationEntries())
                {
                    newRegistryModule.AddRegistrationEntry(registrationEntry);
                }

                foreach (var registrationEntry in otherRegistryModule.GetRegistrationEntries())
                {
                    if (!newRegistryModule.IsRegistered(registrationEntry.Type))
                    {
                        newRegistryModule.AddRegistrationEntry(registrationEntry);
                    }
                }
            }
            catch
            {
                newRegistryModule.Dispose();
                throw;
            }

            return newRegistryModule;
        }

        internal override IEnumerable<RegistrationEntry> GetRegistrationEntries()
        {
            return _RegistrationEntries.Values;
        }

        private void AddRegistrationEntry(RegistrationEntry registrationEntry)
        {
            _RegistrationEntries.Add(registrationEntry.Type, registrationEntry);
        }

        /// <summary>
        /// Registers the specified factory function to create an instance of a certain type.
        /// </summary>
        /// <typeparam name="T">The type that should be registered</typeparam>
        /// <param name="factoryFunc">The factory function that will create the instance.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed in RegistryModule dispose method")]
        internal void Register<T>(Func<T> factoryFunc)
        {
            RegistrationEntry entry = new RegistrationEntry(typeof(T), () => factoryFunc());
            this.AddRegistrationEntry(entry);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            foreach (var registrationEntry in this.GetRegistrationEntries())
            {
                registrationEntry.Dispose();
            }
        }
    }
}
