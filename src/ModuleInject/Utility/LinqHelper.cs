using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Common.Utility;

    public static class LinqHelper
    {
        /// <summary>
        /// Calculates the path of the member and its type for an lambda expression that gives an component/subcomponent
        /// or method/submethod of a module.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="memberPath"></param>
        /// <param name="memberType"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        public static void GetMemberPathAndType(Expression expression, out string memberPath, out Type memberType)
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

        public static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            int depth;
            return GetMemberPath(expression, out depth);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression, out int depth)
        {
            return GetMemberPath((Expression)expression, out depth);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public static string GetMemberPath(Expression expression, out int depth)
        {
            MemberChainEvaluator propertyExtractor = new MemberChainEvaluator();

            IList<Expression> memberExpressions = propertyExtractor.Extract(expression);

            depth = memberExpressions.Count();
            return propertyExtractor.MemberPath;
        }

        public static IList<MethodCallArgument> GetConstructorArguments(LambdaExpression constructorCallExpression)
        {
            CommonFunctions.CheckNullArgument("constructorCallExpression", constructorCallExpression);

            NewExpression constructorExpression = constructorCallExpression.Body as NewExpression;
            if (constructorExpression == null)
            {
                ExceptionHelper.ThrowFormatException("No constructor expression given.");
            }

            return EvaluateArgumentList(constructorExpression.Arguments);
        }

        /// <summary>
        /// Checks the expression and calculates the method name as well as the parameters of the method.
        /// </summary>
        /// It is assumed that the parameters of the method are either fixed values or expression describing components
        /// of a module.
        /// <param name="methodCallExpression"></param>
        /// <param name="methodName"></param>
        /// <param name="arguments"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "General catch is ok because an exception is thrown instead.")]
        public static void GetMethodNameAndArguments(LambdaExpression methodCallExpression, out string methodName,
            out IList<MethodCallArgument> arguments)
        {
            CommonFunctions.CheckNullArgument("methodCallExpression", methodCallExpression);

            arguments = new List<MethodCallArgument>();

            MethodCallExpression methodExpession = methodCallExpression.Body as MethodCallExpression;
            if (methodExpession == null)
            {
                ExceptionHelper.ThrowFormatException("No method call expression given.");
            }

            methodName = methodExpession.Method.Name;

            arguments = EvaluateArgumentList(methodExpession.Arguments);
        }

        private static IList<MethodCallArgument> EvaluateArgumentList(ReadOnlyCollection<Expression> argumentsExpressionList)
        {
            IList<MethodCallArgument> arguments = new List<MethodCallArgument>();

            foreach (var parameter in argumentsExpressionList)
            {
                Expression parameter2 = parameter;
                UnaryExpression unaryExpression = parameter as UnaryExpression;
                if (unaryExpression != null)
                {
                    parameter2 = unaryExpression.Operand;
                }

                switch (parameter2.NodeType)
                {
                    case ExpressionType.MemberAccess:
                    case ExpressionType.Call:
                        {
                            MemberExpression memberExpression = parameter2 as MemberExpression;
                            MethodCallExpression callExpression = parameter2 as MethodCallExpression;
                            if (memberExpression == null && callExpression == null)
                            {
                                ExceptionHelper.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                            }
                            MemberChainEvaluator expEvaluator = new MemberChainEvaluator();
                            try
                            {
                                expEvaluator.Extract(parameter);

                                if (expEvaluator.RootType != null)
                                {
                                    // member access to a parameter of the lambda

                                    arguments.Add(new MethodCallArgument()
                                    {
                                        ArgumentType = expEvaluator.ReturnType,
                                        ResolvePath = expEvaluator.MemberPath,
                                        Value = null
                                    });
                                }
                                else
                                {
                                    // member access to a constant value

                                    object value = EvaluateLinqExpression(parameter);

                                    arguments.Add(new MethodCallArgument()
                                    {
                                        ArgumentType = parameter.Type,
                                        ResolvePath = null,
                                        Value = value
                                    });
                                }
                            }
                            catch
                            {
                                ExceptionHelper.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                            }


                        }
                        break;
                    case ExpressionType.Constant:
                        {
                            ConstantExpression constantExpression = parameter2 as ConstantExpression;
                            if (constantExpression == null)
                            {
                                ExceptionHelper.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                            }
                            arguments.Add(new MethodCallArgument()
                            {
                                ArgumentType = constantExpression.Type,
                                ResolvePath = null,
                                Value = constantExpression.Value
                            });
                        }
                        break;
                    case ExpressionType.New:
                        {
                            NewExpression newExpression = parameter2 as NewExpression;
                            if (newExpression == null)
                            {
                                ExceptionHelper.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                            }

                            object value = EvaluateLinqExpression(newExpression);

                            arguments.Add(new MethodCallArgument()
                            {
                                ArgumentType = newExpression.Type,
                                ResolvePath = null,
                                Value = value
                            });
                        }
                        break;
                    default:
                        ExceptionHelper.ThrowFormatException(Errors.MethodCallArgumentNotSupported, parameter);
                        break;

                }
            }

            return arguments;
        }

        private static object EvaluateLinqExpression(Expression newExpression)
        {
            LambdaExpression lambda = Expression.Lambda(newExpression, null);
            return lambda.Compile().DynamicInvoke();
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
