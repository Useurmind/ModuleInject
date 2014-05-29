using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    internal class MemberChainExtractor : ExpressionVisitor
    {
        private IList<Expression> _memberExpressions;

        public MemberChainExtractor()
        {
            _memberExpressions = new List<Expression>();
        }

        public IList<Expression> Extract(Expression expression)
        {
            Visit(expression);

            return _memberExpressions;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            _memberExpressions.Insert(0, m);

            return base.VisitMemberAccess(m);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            _memberExpressions.Insert(0, m);

            return base.VisitMethodCall(m);
        }
    }
}
