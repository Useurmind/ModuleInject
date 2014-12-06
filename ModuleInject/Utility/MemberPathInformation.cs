using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    public class MemberPathInformation
    {
        public int Depth { get; set; }
        public Type ReturnType { get; set; }
        public string Path { get; set; }
        public Type RootType { get; set; }
        public bool ContainsPropertyAccess { get; set; }
        public bool ContainsMethodCall { get; set; }
        public IEnumerable<MemberInfo> MemberInfos { get; set; }
    }
}
