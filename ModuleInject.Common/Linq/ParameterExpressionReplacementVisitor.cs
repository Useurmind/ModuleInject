using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Common.Linq
{
    public class ParameterExpressionReplacementVisitor : ExpressionVisitor
    {
        private ParameterExpression newParameterExpression;

        public ParameterExpressionReplacementVisitor(ParameterExpression newParameterExpression)
        {
            this.newParameterExpression = newParameterExpression;
        }

        public Expression ReplaceParameter(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            return newParameterExpression;
        }
    }
}
