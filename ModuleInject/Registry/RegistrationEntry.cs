using System.Linq;

namespace ModuleInject.Registry
{
    using System;

    using ModuleInject.Utility;

    /// <summary>
    /// An entry that is contained in a registry module.
    /// </summary>
    internal sealed class RegistrationEntry : DisposableExtBase
    {
        /// <summary>
        /// The _is resolved.
        /// </summary>
        private bool _isResolved;
        private object _resolvedInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationEntry"/> class.
        /// </summary>
        /// <param name="type">
        /// The type that the registration entry describes.
        /// </param>
        /// <param name="factoryFunc">
        /// The factory function to create an instance.
        /// </param>
        public RegistrationEntry(Type type, Func<object> factoryFunc)
        {
            this.Type = type;
            this.FactoryMethod = factoryFunc;
            this._isResolved = false;
        }

        /// <summary>
        /// Gets the type that is registered.
        /// </summary>
        /// <value>
        /// The type that is registered.
        /// </value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the resolved instance.
        /// </summary>
        /// <value>
        /// The resolved instance.
        /// </value>
        public object ResolvedInstance
        {
            get
            {
                this.Resolve();
                return _resolvedInstance;
            }
        }

        /// <summary>
        /// Gets the factory method that is used to create the resolved instance.
        /// </summary>
        /// <value>
        /// The factory method that is used to create the resolved instance.
        /// </value>
        public Func<object> FactoryMethod { get; private set; }

        /// <summary>
        /// Creates the resolved instance if not yet done.
        /// </summary>
        public void Resolve()
        {
            if (!this._isResolved)
            {
                _resolvedInstance = this.FactoryMethod();
                this._isResolved = true;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this._isResolved)
            {
                IDisposable disposable = _resolvedInstance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                    _resolvedInstance = null;
                    _isResolved = false;
                }
            }
        }
    }
}