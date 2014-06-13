using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    internal static class LinqHelper
    {
        internal static void GetMemberPathAndType(Expression expression, out string memberPath, out Type memberType)
        {
            memberType = null;
            int depth;
            memberPath = GetMemberPath(expression, out depth);
            LambdaExpression lambdaExpression = (LambdaExpression)expression;

            memberType = TryGetMemberTypeFromMemberOrMethod(lambdaExpression.Body);
            if (memberType != null)
            {
                return;
            }

            UnaryExpression unaryExpression = lambdaExpression.Body as UnaryExpression;
            if (unaryExpression != null && unaryExpression.NodeType == ExpressionType.Convert)
            {
                memberType = TryGetMemberTypeFromMemberOrMethod(unaryExpression.Operand);
                return;
            }

            throw new ModuleInjectException("Expression must describe a property or method.");
        }

        internal static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            int depth;
            return GetMemberPath(expression, out depth);
        }

        internal static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression, out int depth)
        {
            return GetMemberPath((Expression)expression, out depth);
        }

        internal static string GetMemberPath(Expression expression, out int depth)
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

        private static Type TryGetMemberTypeFromMemberOrMethod(Expression expression)
        {
            Type memberType = null;

            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                memberType = ((PropertyInfo)memberExpression.Member).PropertyType;
            }

            MethodCallExpression methodExpression = expression as MethodCallExpression;
            if (methodExpression != null)
            {
                memberType = methodExpression.Method.ReturnType;
            }

            return memberType;
        }
    }
}
