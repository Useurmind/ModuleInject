using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    public class PropertyChainExtractor : ExpressionVisitor
    {
        private IList<MemberExpression> _memberExpressions;

        public PropertyChainExtractor()
        {
            _memberExpressions = new List<MemberExpression>();
        }

        public IList<MemberExpression> Extract(Expression expression)
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
            throw new ModuleInjectException("Only Property Accesses are allowed.");
        }
    }
}
