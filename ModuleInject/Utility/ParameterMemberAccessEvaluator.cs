using ModuleInject.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Utility
{
    /// <summary>
    /// Evaluates a lambda expression for access to members of a specified parameter of the expression.
    /// </summary>
    public class ParameterMemberAccessEvaluator : ExpressionVisitor
    {
        private LambdaExpression expression;
        private int parameterIndex;

        private Stack<Expression> expressionStack;

        private IList<MemberPathInformation> memberPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterMemberAccessEvaluator"/> class.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="parameterIndex">Index of the parameter of the lambda for which the evaluation should be done.</param>
        public ParameterMemberAccessEvaluator(LambdaExpression expression, int parameterIndex)
        {
            this.expression = expression;
            this.parameterIndex = parameterIndex;

            expressionStack = new Stack<Expression>();
            memberPaths = new List<MemberPathInformation>();
        }

        override OnV

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
        }
    }
}
