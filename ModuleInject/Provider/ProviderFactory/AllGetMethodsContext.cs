using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Common.Linq;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    public interface IAllGetMethodsContext
    {
        /// <param name="filter">Should return true for all methods that should be included.</param>
        IAllGetMethodsContext Where(Func<MethodInfo, bool> filter);

        IAllGetMethodsContext ExceptFrom(Type type, bool recursive = false);

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

        public IAllGetMethodsContext ExceptFrom(Type type, bool recursive = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (!recursive)
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
        public static IAllGetMethodsContext ExceptFrom<TExcept>(this IAllGetMethodsContext context, bool recursive = false)
        {
            return context.ExceptFrom(typeof(TExcept), recursive);
        }
    }
}
