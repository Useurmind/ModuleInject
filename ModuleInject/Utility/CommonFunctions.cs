using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    internal static class CommonFunctions
    {
        internal static void CheckNullArgument<TArgument>(string name, [ValidatedNotNull]TArgument argument)
        where TArgument : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void ThrowPropertyAndTypeException<TModule>(string errorMessage, string propName)
        {
            throw new ModuleInjectException(string.Format(CultureInfo.CurrentCulture, errorMessage, propName, typeof(TModule).FullName));
        }

        internal static void ThrowTypeException<TModule>(string errorMessage, params object[] param)
        {
            ThrowTypeException(typeof(TModule), errorMessage, param);
        }

        internal static void ThrowTypeException(Type moduleType, string errorMessage, params object[] param)
        {
            object[] formatParams = new object[] { moduleType.FullName };
            formatParams = formatParams.Concat(param).ToArray();

            throw new ModuleInjectException(string.Format(CultureInfo.CurrentCulture, errorMessage, formatParams));
        }
    }
}
