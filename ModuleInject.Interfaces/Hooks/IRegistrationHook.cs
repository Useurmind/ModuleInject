using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Hooks
{
	/// <summary>
	/// Interface for hooks that are executed when a component/factory is registered.
	/// </summary>
	public interface IRegistrationHook
	{
		/// <summary>
		/// Should the hook be used inside the given module.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <returns>True if the hook should be applied to the module, else false.</returns>
		bool AppliesToModule(IModule module);

		/// <summary>
		/// Should the hook be executed for a given registration.
		/// </summary>
		/// <remarks>
		/// If a hook applies to all modules, this method is performed for each and every registration
		/// that you do in your dependency injection process.
		/// Therefore, be cautios about using hooks and make this method as performant as possible.
		/// </remarks>
		/// <param name="injectionRegister">The registration context of the registration.</param>
		/// <returns>True if the hook should be executed.</returns>
		bool AppliesToRegistration(IInjectionRegister injectionRegister);

		/// <summary>
		/// Executes the hook for the registration of a component/factory.
		/// </summary>
		/// <param name="injectionRegister">The registration context of the registration.</param>
		void Execute(IInjectionRegister injectionRegister);
	}
}
