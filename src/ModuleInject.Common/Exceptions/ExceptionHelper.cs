namespace ModuleInject.Common.Exceptions
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class ExceptionHelper
    {
        public static void ThrowPropertyAndTypeException<TModule>(string errorMessage, string propName)
        {
            ThrowPropertyAndTypeException(typeof(TModule), errorMessage, propName);
        }


        public static void ThrowPropertyAndTypeException(Type moduleType, string errorMessage, string propName)
        {
            ThrowFormatException(errorMessage, propName, moduleType.FullName);
        }

        public static void ThrowTypeException<TModule>(string errorMessage, params object[] param)
        {
            ThrowTypeException(typeof(TModule), errorMessage, param);
        }

        public static void ThrowTypeException(Type moduleType, string errorMessage, params object[] param)
        {
            object[] formatParams = new object[] { moduleType.FullName };
            formatParams = formatParams.Concat(param).ToArray();

            ThrowFormatException(errorMessage, formatParams);
        }

        public static void ThrowFormatException(string errorMessage, params object[] param)
        {
            throw new ModuleInjectException(string.Format(CultureInfo.CurrentCulture, errorMessage, param));
        }
    }
}