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
        /// <summary>
        /// Calculates the path of the member and its type for an lambda expression that gives an component/subcomponent
        /// or method/submethod of a module.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="memberPath"></param>
        /// <param name="memberType"></param>
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

                if (!unaryExpression.Type.IsAssignableFrom(memberType))
                {
                    throw new ModuleInjectException("The given cast is not valid, the property can not be assigned to the type in the cast.");
                }
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
            MemberChainEvaluator propertyExtractor = new MemberChainEvaluator();

            IList<Expression> memberExpressions = propertyExtractor.Extract(expression);

            depth = memberExpressions.Count();
            return propertyExtractor.MemberPath;
        }

        /// <summary>
        /// Checks the expression and calculates the method name as well as the parameters of the method.
        /// </summary>
        /// It is assumed that the parameters of the method are either fixed values or expression describing components
        /// of a module.
        /// <param name="methodCallExpression"></param>
        /// <param name="methodName"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification="General catch is ok because an exception is thrown instead." )]
        internal static void GetMethodNameAndArguments(LambdaExpression methodCallExpression, out string methodName,
            out IList<MethodCallArgument> arguments)
        {
            arguments = new List<MethodCallArgument>();

            MethodCallExpression methodExpession = methodCallExpression.Body as MethodCallExpression;
            if (methodExpession == null)
            {
                throw new ModuleInjectException("No method call expression given.");
            }

            methodName = methodExpession.Method.Name;

            foreach (var parameter in methodExpession.Arguments)
            {
                Expression parameter2 = parameter;
                UnaryExpression unaryExpression = parameter as UnaryExpression;
                if (unaryExpression != null)
                {
                    parameter2 = unaryExpression.Operand;
                }

                MemberExpression memberExpression = parameter2 as MemberExpression;
                if (memberExpression != null)
                {
                    MemberChainEvaluator expEvaluator = new MemberChainEvaluator();
                    try
                    {
                        expEvaluator.Extract(parameter);
                    }
                    catch
                    {
                        CommonFunctions.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                    }

                    arguments.Add(new MethodCallArgument()
                    {
                        ArgumentType = expEvaluator.ReturnType,
                        ResolvePath = expEvaluator.MemberPath,
                        Value = null
                    });
                }
                else
                {

                    ConstantExpression constantExpression = parameter2 as ConstantExpression;
                    if (constantExpression == null)
                    {
                        CommonFunctions.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                    }
                    arguments.Add(new MethodCallArgument()
                    {
                        ArgumentType = constantExpression.Type,
                        ResolvePath = null,
                        Value = constantExpression.Value
                    });
                }

            }
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
