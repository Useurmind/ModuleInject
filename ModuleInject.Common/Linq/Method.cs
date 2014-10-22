using System.Linq;

namespace ModuleInject.Common.Linq
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;

    public class Method
    {
        private MethodInfo _methodInfo;

        private Method(MethodInfo methodInfo)
        {
            this._methodInfo = methodInfo;
        }

        public static Method Get<TObject, TMethodReturn>(Expression<Func<TObject, TMethodReturn>> methodExpression)
        {
            CommonFunctions.CheckNullArgument("methodExpression", methodExpression);

            CheckExpression(methodExpression);

            MethodCallExpression memberExpression = (MethodCallExpression)methodExpression.Body;
            MethodInfo methodInfo = (MethodInfo)memberExpression.Method;
            return new Method(methodInfo);
        }

        public static Method Get<TObject>(Expression<Action<TObject>> methodExpression)
        {
            CommonFunctions.CheckNullArgument("methodExpression", methodExpression);

            CheckExpression(methodExpression);

            MethodCallExpression memberExpression = (MethodCallExpression)methodExpression.Body;
            MethodInfo methodInfo = (MethodInfo)memberExpression.Method;
            return new Method(methodInfo);
        }

        private static void CheckExpression(LambdaExpression methodExpression)
        {
            Exception exception = new ModuleInjectException("Methods can only be constructed from expressions describing direct methods of an object.");

            ParameterExpression parameterExpression = methodExpression.Parameters[0];

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

            ParameterExpression paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(memberExpression.Object, parameterExpression.Type);
            if (paramExp == null)
            {
                throw exception;
            }
        }

        public static implicit operator string(Method method)
        {
            CommonFunctions.CheckNullArgument("method", method);

            return method.ToString();
        }

        public override string ToString()
        {

            return this._methodInfo.Name;
        }
    }
}
