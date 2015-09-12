using System.Linq;

namespace ModuleInject.Common.Utility
{
    using System;

    public static class CommonFunctions
    {
        public static void CheckNullArgument<TArgument>(string name, [ValidatedNotNull]TArgument argument)
        where TArgument : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
