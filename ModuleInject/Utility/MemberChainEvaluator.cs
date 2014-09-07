using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    /// <summary>
    /// This class evaluates an expression describing a chain of access to members of a class.
    /// </summary>
    /// Expression may look like this:
    /// - x.Property1
    /// - x.Property1.Property2
    /// - x.Method1()
    /// - x.Method1().Property3
    /// - (Type1)x.Property1
    /// x must be a ParameterExpression.
    internal class MemberChainEvaluator : ExpressionVisitor
    {
        private string _memberPath;
        private IList<Expression> _memberExpressions;
        private Expression _lastMemberExpression;

        /// <summary>
        /// The expression in evaluation.
        /// </summary>
        public Expression EvaluatedExpression { get; private set; }
        /// <summary>
        /// If required an automatic check can be performed on the type of 
        /// the object on which the member accesses are performed.
        /// </summary>
        /// Exception is thrown on mismatch.
        public Type TargetRootType { get; set; }

        /// <summary>
        /// The type of the last member in the chain.
        /// </summary>
        public Type MemberType { get; private set; }
        /// <summary>
        /// The type that is finally returned by the expression (e.g. in case of a cast).
        /// </summary>
        public Type ReturnType { get; private set; }

        /// <summary>
        /// The name of the root object.
        /// </summary>
        public string RootName { get; private set; }
        /// <summary>
        /// The type of the root object.
        /// </summary>
        public Type RootType { get; private set; }

        /// <summary>
        /// Gives the complete path from the root to the deepest member,
        /// e.g. on x.Property1.Property2 it gives "Property1.Property2"
        /// </summary>
        public string MemberPath
        {
            get
            {
                if (_memberPath == null)
                {
                    var propertyNames = _memberExpressions.Select(me =>
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
                    _memberPath = string.Join(".", propertyNames);
                }
                return _memberPath;
            }
        }

        public MemberChainEvaluator()
        {
            _memberExpressions = new List<Expression>();
        }

        public IList<Expression> Extract(Expression expression)
        {
            EvaluatedExpression = expression;

            Visit(expression);

            return _memberExpressions;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            CheckChainIsContinuous(m);

            if (_lastMemberExpression == null)
            {
                MemberType = m.Type;
                SetReturnType();
            }

            _memberExpressions.Insert(0, m);

            _lastMemberExpression = m;

            return base.VisitMemberAccess(m);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            CheckChainIsContinuous(m);

            if (_lastMemberExpression == null)
            {
                MemberType = m.Method.ReturnType;
                SetReturnType();
            }

            _memberExpressions.Insert(0, m);

            _lastMemberExpression = m;

            return base.VisitMethodCall(m);
        }

        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            CheckChainIsContinuous(parameterExpression);

            RootName = parameterExpression.Name;
            RootType = parameterExpression.Type;

            if (TargetRootType != null && RootType != TargetRootType)
            {
                CommonFunctions.ThrowFormatException(Errors.MemberChainEvaluator_RootTypeMismatch, EvaluatedExpression);
            }

            return base.VisitParameter(parameterExpression);
        }

        protected override Expression VisitUnary(UnaryExpression unaryExpression)
        {
            if (unaryExpression.NodeType == ExpressionType.Convert)
            {
                ReturnType = unaryExpression.Type;
            }
            return base.VisitUnary(unaryExpression);
        }

        private void CheckChainIsContinuous(Expression currentExpression)
        {
            if (_lastMemberExpression == null)
                return;

            MemberExpression memberExpression = _lastMemberExpression as MemberExpression;
            MethodCallExpression methodExpression = _lastMemberExpression as MethodCallExpression;
            Expression checkedExpression = null;
            if (memberExpression != null)
            {
                checkedExpression = memberExpression.Expression;
            }
            else if (methodExpression != null)
            {
                checkedExpression = methodExpression.Object;
            }

            if (checkedExpression.NodeType == ExpressionType.MemberAccess || checkedExpression.NodeType == ExpressionType.Call)
            {
                if (checkedExpression != currentExpression)
                {
                    ThrowMemberChainNotContinuous();
                }
            }
            else
            {
                ParameterExpression paramExpression = LinqHelper.GetParameterExpressionWithPossibleConvert(checkedExpression, TargetRootType);

                if (paramExpression == null || paramExpression != currentExpression)
                {
                    ThrowMemberChainNotContinuous();
                }
            }
        }

        private void ThrowMemberChainNotContinuous()
        {
            CommonFunctions.ThrowFormatException(Errors.MemberChainEvaluator_MemberChainNotContinuous, EvaluatedExpression);
        }

        private void SetReturnType()
        {
            ReturnType = ReturnType == null ? MemberType : ReturnType;
        }
    }
}
