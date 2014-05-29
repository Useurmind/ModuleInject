using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Utility
{
    public static class LinqHelper
    {
        public static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> exp)
        {
            MemberChainExtractor propertyExtractor = new MemberChainExtractor();

            IList<Expression> memberExpressions = propertyExtractor.Extract(exp);

            var propertyNames = memberExpressions.Select(me =>{
                MemberExpression memberExp = me as MemberExpression;
                if(memberExp != null) {
                    return memberExp.Member.Name;
                }

                MethodCallExpression methodCallExp = me as MethodCallExpression;
                if(methodCallExp != null) {
                    return methodCallExp.Method.Name;
                }

                throw new ModuleInjectException("Invalid expression in member chain.");
            });

            return string.Join(".", propertyNames);
        }
    }
}
