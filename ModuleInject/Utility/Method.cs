using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    public class Method
    {
        private MethodInfo _methodInfo;

        private Method(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

        public static Method Get<TObject, TMethodReturn>(Expression<Func<TObject, TMethodReturn>> methodExpression)
        {
            if (methodExpression == null)
            {
                throw new ArgumentNullException();
            }

            CheckExpression(methodExpression);

            MethodCallExpression memberExpression = (MethodCallExpression)methodExpression.Body;
            MethodInfo methodInfo = (MethodInfo)memberExpression.Method;
            return new Method(methodInfo);
        }

        private static void CheckExpression<TObject, TMethodReturn>(Expression<Func<TObject, TMethodReturn>> methodExpression)
        {
            Exception exception = new ModuleInjectException("Methods can only be constructed from expressions describing direct methods of an object.");

            MethodCallExpression memberExpression = methodExpression.Body as MethodCallExpression;
            if (memberExpression == null)
            {
                throw exception;
            }

            MethodInfo propInfo = memberExpression.Method as MethodInfo;
            if (propInfo == null)
            {
                throw exception;
            }

            ParameterExpression paramExp = memberExpression.Object as ParameterExpression;
            if (paramExp == null)
            {
                throw exception;
            }
        }

        public static implicit operator string(Method method)
        {
            if (method == null)
            {
                return null;
            }

            return method._methodInfo.Name;
        }
    }
}
