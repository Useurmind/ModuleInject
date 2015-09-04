using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Common.Linq
{
    public class MemberNameComparer : IEqualityComparer<MemberInfo>
    {
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(MemberInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class PropertyNameComparer : IEqualityComparer<PropertyInfo>
    {
        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(PropertyInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class MethodNameComparer : IEqualityComparer<MethodInfo>
    {
        public bool Equals(MethodInfo x, MethodInfo y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(MethodInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
