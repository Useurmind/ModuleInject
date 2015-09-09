using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Common.Linq;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    /// <summary>
    /// This context is used to filter and add the get methods of an instance as a service source to a service provider.
    /// </summary>
    /// <remarks>
    /// "Get methods" are methods that
    /// - have no parameters
    /// - have a non void return type
    /// </remarks>
    public interface IAllGetMethodsContext
    {
        /// <summary>
        /// From all remaining methods only keep the ones that fullfill the given predicate.
        /// </summary>
        /// <param name="filter">Should return true for all methods that should be included.</param>
        IAllGetMethodsContext Where(Func<MethodInfo, bool> filter);

        /// <summary>
        /// Exclude the get methods that are declared on the given type (or further up the inheritance chain).
        /// </summary>
        /// <param name="type">The type from which get methods should be excluded.</param>
        /// <param name="inherited">If set to true all get methods up the inheritance chain starting from the given type are excluded.</param>
        IAllGetMethodsContext ExceptFrom(Type type, bool inherited = false);

        /// <summary>
        /// Confirm that the chosen set of get methods should be added to the service provider.
        /// </summary>
        /// <returns></returns>
        IFromInstanceContext Extract();
    }

    public class AllGetMethodsContext : IAllGetMethodsContext
    {
        private readonly FromInstanceContext fromInstance;
        private IEnumerable<MethodInfo> methods;

        public AllGetMethodsContext(FromInstanceContext fromInstance)
        {
            this.fromInstance = fromInstance;
            this.methods = fromInstance.Instance.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.ReturnType != typeof(void) && 
                            x.GetParameters().Length == 0 &&
                            !x.IsSpecialName);
        }

        /// <param name="filter">Should return true for all methods that should be included.</param>
        public IAllGetMethodsContext Where(Func<MethodInfo, bool> filter)
        {
            methods = methods.Where(filter);
            return this;
        }

        public IAllGetMethodsContext ExceptFrom(Type type, bool inherited = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (!inherited)
            {
                bindingFlags |= BindingFlags.DeclaredOnly;
            }

            var exceptMethods = type.GetMethods(bindingFlags);

            methods = methods.Except(exceptMethods, new MethodNameComparer());

            return this;
        }

        public IFromInstanceContext Extract()
        {
            foreach (var method in methods)
            {
                fromInstance.ServiceProvider.AddServiceSource(new MethodServiceSource(fromInstance.Instance, method));
            }
            return fromInstance;
        }
    }

    public static class AllGetMethodsContextExtensions
    {
        /// <summary>
        /// Exclude the get methods that are declared on the given type (or further up the inheritance chain).
        /// </summary>
        /// <typeparam name="TExcept">The type from which get methods should be excluded.</typeparam>
        /// <param name="inherited">If set to true all get methods up the inheritance chain starting from the given type are excluded.</param>
        public static IAllGetMethodsContext ExceptFrom<TExcept>(this IAllGetMethodsContext context, bool inherited = false)
        {
            return context.ExceptFrom(typeof(TExcept), inherited);
        }
    }
}
