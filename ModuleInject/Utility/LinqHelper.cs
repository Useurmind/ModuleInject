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
            return Property.Get(exp);
        }
    }
}
