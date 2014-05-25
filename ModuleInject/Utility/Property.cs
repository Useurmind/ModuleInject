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

        public static Property Get<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            CheckExpression(propertyExpression);

            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            PropertyInfo propInfo = (PropertyInfo)memberExpression.Member;
            return new Property(propInfo);
        }

        private static void CheckExpression<TObject, TProperty>(Expression<Func<TObject, TProperty>> propertyExpression)
        {
            Exception ex = new ModuleInjectException("Properties can only be constructed from expressions desribing direct properties of an object.");

            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw ex;
            }

            PropertyInfo propInfo = memberExpression.Member as PropertyInfo;
            if(propInfo == null)
            {
                throw ex;
            }

            ParameterExpression paramExp = memberExpression.Expression as ParameterExpression;
            if(paramExp == null)
            {
                throw ex;
            }
        }

        public static implicit operator string(Property property) {
            return property._propertyInfo.Name;
        }
    }
}
