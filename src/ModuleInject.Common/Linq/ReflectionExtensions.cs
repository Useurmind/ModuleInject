namespace ModuleInject.Common.Linq
{
    using System;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static bool HasCustomAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return memberInfo.HasCustomAttribute(typeof(TAttribute));
        }

        public static bool HasCustomAttribute(this MemberInfo memberInfo, Type attributeType)
        {
            return memberInfo.GetCustomAttributes(attributeType, false).Length > 0;
        }
    }
}