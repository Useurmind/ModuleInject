using System.Linq;

namespace ModuleInject.Common.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;

    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification="Externally created code.", Scope="method")]
    public abstract class ExpressionVisitor
    {
        protected ExpressionVisitor()
        {
        }

        protected virtual Expression Visit(Expression expression)
        {
            if (expression == null)
                return expression;
            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)expression);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)expression);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)expression);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)expression);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)expression);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)expression);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)expression);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)expression);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)expression);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)expression);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)expression);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)expression);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)expression);
                default:
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unhandled expression type: '{0}'", expression.NodeType));
            }
        }

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }
            return initializer;
        }

        protected virtual Expression VisitUnary(UnaryExpression unaryExpression)
        {
            Expression operand = this.Visit(unaryExpression.Operand);
            if (operand != unaryExpression.Operand)
            {
                return Expression.MakeUnary(unaryExpression.NodeType, operand, unaryExpression.Type, unaryExpression.Method);
            }
            return unaryExpression;
        }

        protected virtual Expression VisitBinary(BinaryExpression binaryExpression)
        {
            Expression left = this.Visit(binaryExpression.Left);
            Expression right = this.Visit(binaryExpression.Right);
            Expression conversion = this.Visit(binaryExpression.Conversion);
            if (left != binaryExpression.Left || right != binaryExpression.Right || conversion != binaryExpression.Conversion)
            {
                if (binaryExpression.NodeType == ExpressionType.Coalesce && binaryExpression.Conversion != null)
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                else
                    return Expression.MakeBinary(binaryExpression.NodeType, left, right, binaryExpression.IsLiftedToNull, binaryExpression.Method);
            }
            return binaryExpression;
        }

        protected virtual Expression VisitTypeIs(TypeBinaryExpression typeBinaryExpression)
        {
            Expression expr = this.Visit(typeBinaryExpression.Expression);
            if (expr != typeBinaryExpression.Expression)
            {
                return Expression.TypeIs(expr, typeBinaryExpression.TypeOperand);
            }
            return typeBinaryExpression;
        }

        protected virtual Expression VisitConstant(ConstantExpression constantExpression)
        {
            return constantExpression;
        }

        protected virtual Expression VisitConditional(ConditionalExpression conditionalExpression)
        {
            Expression test = this.Visit(conditionalExpression.Test);
            Expression ifTrue = this.Visit(conditionalExpression.IfTrue);
            Expression ifFalse = this.Visit(conditionalExpression.IfFalse);
            if (test != conditionalExpression.Test || ifTrue != conditionalExpression.IfTrue || ifFalse != conditionalExpression.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }
            return conditionalExpression;
        }

        protected virtual Expression VisitParameter(ParameterExpression parameterExpression)
        {
            return parameterExpression;
        }

        protected virtual Expression VisitMemberAccess(MemberExpression memberExpression)
        {
            Expression exp = this.Visit(memberExpression.Expression);
            if (exp != memberExpression.Expression)
            {
                return Expression.MakeMemberAccess(exp, memberExpression.Member);
            }
            return memberExpression;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            Expression obj = this.Visit(methodCallExpression.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(methodCallExpression.Arguments);
            if (obj != methodCallExpression.Object || args != methodCallExpression.Arguments)
            {
                return Expression.Call(obj, methodCallExpression.Method, args);
            }
            return methodCallExpression;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = this.Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(p);
                }
            }
            if (list != null)
            {
                return list.AsReadOnly();
            }
            return original;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression e = this.Visit(assignment.Expression);
            if (e != assignment.Expression)
            {
                return Expression.Bind(assignment.Member, e);
            }
            return assignment;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
            if (bindings != binding.Bindings)
            {
                return Expression.MemberBind(binding.Member, bindings);
            }
            return binding;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
            if (initializers != binding.Initializers)
            {
                return Expression.ListBind(binding.Member, initializers);
            }
            return binding;
        }

        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                MemberBinding b = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(b);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                ElementInit init = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(init);
                }
            }
            if (list != null)
                return list;
            return original;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression body = this.Visit(lambda.Body);
            if (body != lambda.Body)
            {
                return Expression.Lambda(lambda.Type, body, lambda.Parameters);
            }
            return lambda;
        }

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
            if (args != nex.Arguments)
            {
                if (nex.Members != null)
                    return Expression.New(nex.Constructor, args, nex.Members);
                else
                    return Expression.New(nex.Constructor, args);
            }
            return nex;
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
            if (n != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }
            return init;
        }

        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
            if (n != init.NewExpression || initializers != init.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }
            return init;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression newArrayExpression)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(newArrayExpression.Expressions);
            if (exprs != newArrayExpression.Expressions)
            {
                if (newArrayExpression.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(newArrayExpression.Type.GetElementType(), exprs);
                }
                else
                {
                    return Expression.NewArrayBounds(newArrayExpression.Type.GetElementType(), exprs);
                }
            }
            return newArrayExpression;
        }

        protected virtual Expression VisitInvocation(InvocationExpression invocationExpression)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(invocationExpression.Arguments);
            Expression expr = this.Visit(invocationExpression.Expression);
            if (args != invocationExpression.Arguments || expr != invocationExpression.Expression)
            {
                return Expression.Invoke(expr, args);
            }
            return invocationExpression;
        }
    }
}
