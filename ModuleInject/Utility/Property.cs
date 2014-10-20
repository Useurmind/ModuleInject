using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    public class Property
    {
        private PropertyInfo _propertyInfo;

        private Property(PropertyInfo propInfo)
        {
            _propertyInfo = propInfo;
        }

        public void SetValue<TObject, TValue>(TObject instance, TValue value)
        {
            _propertyInfo.SetValue(instance, value, null);
        }

        public T GetValue<T>(object instance)
        {
            return (T)_propertyInfo.GetValue(instance, null);
        }

        internal static Property Get(Expression propertyExpression)
        {
            CommonFunctions.CheckNullArgument("propertyExpression", propertyExpression);

            CheckExpression(propertyExpression);

            LambdaExpression lambdaExpression = (LambdaExpression)propertyExpression;
            MemberExpression memberExpression = (MemberExpression)lambdaExpression.Body;
            PropertyInfo propInfo = (PropertyInfo)memberExpression.Member;
            return new Property(propInfo);
        }

        public static Property Get<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            return Get((Expression)propertyExpression);
        }

        private static void CheckExpression(Expression propertyExpression)
        {
            Exception exception = new ModuleInjectException("Properties can only be constructed from expressions describing direct properties of an object.");

            LambdaExpression lambdaExpression = (LambdaExpression)propertyExpression;
            ParameterExpression parameterExpression = lambdaExpression.Parameters[0];

            MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw exception;
            }

            PropertyInfo propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw exception;
            }

            ParameterExpression paramExp = LinqHelper.GetParameterExpressionWithPossibleConvert(memberExpression.Expression, parameterExpression.Type);
            if (paramExp == null)
            {
                throw exception;
            }
        }

        public static implicit operator string(Property property)
        {
            CommonFunctions.CheckNullArgument("property", property);

            return property.ToString();
        }

        public override string ToString()
        {
            return _propertyInfo.Name;
        }
    }
}
