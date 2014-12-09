using ModuleInject.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    /// <summary>
    /// Evaluates a lambda expression for access to members of a specified parameter of the expression or a given object.
    /// </summary>
    public class ParameterMemberAccessEvaluator : InterceptingExpressionVisitor
    {
        private LambdaExpression expression;
        private ParameterExpression evaluatedParameter;

        private Stack<Expression> expressionStack;

        private IList<MemberPathInformation> memberPaths;
        private object instance;


        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterMemberAccessEvaluator"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="parameterIndex">Index of the parameter of the lambda for which the evaluation should be done.</param>
        /// <param name="instance">An instance for which the evaluation should be done.</param>
        public ParameterMemberAccessEvaluator(LambdaExpression expression, int parameterIndex, object instance)
        {
            this.expression = expression;
            this.evaluatedParameter = expression.Parameters[parameterIndex];
            this.instance = instance;

            expressionStack = new Stack<Expression>();
            memberPaths = new List<MemberPathInformation>();
        }

        public IEnumerable<MemberPathInformation> MemberPaths {  get { return memberPaths; } }

        public void Evaluate()
        {
            this.Visit(this.expression);
        }

        protected override void OnVisiting(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                    EvaluateMemberAccess(expression);
                    break;
                case ExpressionType.Parameter:
                    EvaluateParameterMemberAccess((ParameterExpression)expression);
                    break;
                case ExpressionType.Constant:
                    EvaluateConstantMemberAccess((ConstantExpression)expression);
                    break;
                default:
                    // chain of member access broken, so clear the stack
                    expressionStack.Clear();
                    break;
            }

            base.OnVisiting(expression);
        }

        private void EvaluateMemberAccess(Expression expression)
        {
            expressionStack.Push(expression);
        }

        private void EvaluateConstantMemberAccess(ConstantExpression constantExpression)
        {
            if(constantExpression.Value != this.instance)
            {
                expressionStack.Clear();
                return;
            }

            EvaluateExpressionStack(this.instance.GetType());
        }

        private void EvaluateParameterMemberAccess(ParameterExpression parameterExpression)
        {
            if (parameterExpression != evaluatedParameter)
            {
                expressionStack.Clear();
                return;
            }

            EvaluateExpressionStack(this.evaluatedParameter.Type);
        }

        private void EvaluateExpressionStack(Type rootType)
        {
            if (expressionStack.Count == 0)
            {
                return;
            }

            var names = new string[expressionStack.Count];
            var memberInfos = new MemberInfo[expressionStack.Count];
            int index = 0;
            Type returnType = null;
            bool containsPropertyAccess = false;
            bool containsMethodCall = false;

            while (expressionStack.Any())
            {
                var expression = expressionStack.Pop();

                MemberExpression memberExpression = expression as MemberExpression;
                if (memberExpression != null)
                {
                    names[index] = memberExpression.Member.Name;
                    memberInfos[index] = memberExpression.Member;
                    if (!expressionStack.Any())
                    {
                        returnType = ((PropertyInfo)memberExpression.Member).PropertyType;
                    }
                    containsPropertyAccess = true;
                }

                MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                if (methodCallExpression != null)
                {
                    names[index] = methodCallExpression.Method.Name;
                    memberInfos[index] = methodCallExpression.Method;
                    if (!expressionStack.Any())
                    {
                        returnType = methodCallExpression.Method.ReturnType;
                    }

                    containsMethodCall = true;
                }

                index++;
            }

            var pathInfo = new MemberPathInformation()
            {
                RootType = rootType,
                ReturnType = returnType,
                Depth = names.Count(),
                Path = String.Join(".", names),
                ContainsMethodCall = containsMethodCall,
                ContainsPropertyAccess = containsPropertyAccess,
                MemberInfos = memberInfos
            };

            memberPaths.Add(pathInfo);
        }
    }
}
