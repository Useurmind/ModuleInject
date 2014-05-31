using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Utility
{
    internal static class LinqHelper
    {
        internal static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            int depth;
            return GetMemberPath(expression, out depth);
        }

        internal static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression, out int depth)
        {
            MemberChainExtractor propertyExtractor = new MemberChainExtractor();

            IList<Expression> memberExpressions = propertyExtractor.Extract(expression);

            var propertyNames = memberExpressions.Select(me =>
            {
                MemberExpression memberExp = me as MemberExpression;
                if (memberExp != null)
                {
                    return memberExp.Member.Name;
                }

                MethodCallExpression methodCallExp = me as MethodCallExpression;
                if (methodCallExp != null)
                {
                    return methodCallExp.Method.Name;
                }

                throw new ModuleInjectException("Invalid expression in member chain.");
            });

            depth = propertyNames.Count();
            return string.Join(".", propertyNames);
        }
    }
}
