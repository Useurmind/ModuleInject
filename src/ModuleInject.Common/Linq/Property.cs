﻿using System.Linq;

namespace ModuleInject.Common.Linq
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;

    public class Property
    {
        private PropertyInfo _propertyInfo;

        private Property(PropertyInfo propInfo)
        {
            this._propertyInfo = propInfo;
        }

        public void SetValue<TObject, TValue>(TObject instance, TValue value)
        {
            this._propertyInfo.SetValue(instance, value, null);
        }

        public T GetValue<T>(object instance)
        {
            return (T)this._propertyInfo.GetValue(instance, null);
        }

        public static Property Get(Expression propertyExpression)
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

            ParameterExpression paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(memberExpression.Expression, parameterExpression.Type);
            if (paramExp == null)
            {
                throw exception;
            }
        }

        public static implicit operator string (Property property)
        {
            CommonFunctions.CheckNullArgument("property", property);

            return property.ToString();
        }

        public override string ToString()
        {
            return this._propertyInfo.Name;
        }
    }

    public static class PropertyExtensions {
        public static Property GetProperty<TObject, TProperty>(this TObject instance, Expression<Func<TObject, TProperty>> propertyExpression)
        {
            return Property.Get(propertyExpression);
        }
    }
}
