using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Utility
{
    public static class LinqHelper
    {
        public static string GetMemberPath<TObject, TProperty>(Expression<Func<TObject, TProperty>> exp)
        {
            PropertyChainExtractor propertyExtractor = new PropertyChainExtractor();

            IList<MemberExpression> memberExpressions = propertyExtractor.Extract(exp);

            var propertyNames = memberExpressions.Select(me => me.Member.Name);

            return string.Join(".", propertyNames);
        }
    }
}
