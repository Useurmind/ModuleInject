using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Utility
{
    internal static class CommonFunctions
    {
        internal static void ThrowPropertyAndTypeException<TModule>(string errorMessage, string propName)
        {
            throw new ModuleInjectException(string.Format(errorMessage, propName, typeof(TModule).FullName));
        }

        internal static void ThrowTypeException<TModule>(string errorMessage, params object[] param)
        {
            object[] formatParams = new object[] { typeof(TModule).FullName };
            formatParams = formatParams.Concat(param).ToArray();

            throw new ModuleInjectException(string.Format(errorMessage, formatParams));
        }
    }
}
