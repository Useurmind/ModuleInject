using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleInject.Modules.Fluent
{
    internal class ModifiedDependency
    {
        public string MemberPath { get; private set; }
        public Type MemberType { get; private set; }
        public IList<Action<object>> ModifyActions { get; private set; }

        public ModifiedDependency(string memberPath, Type memberType, Action<object> modifyAction)
        {
            this.MemberPath = memberPath;
            this.MemberType = memberType;
            this.ModifyActions = new List<Action<object>>();
            this.AddModifyAction(modifyAction);
        }

        public void AddModifyAction(Action<object> modifyAction)
        {
            this.ModifyActions.Add(modifyAction);
        }
    }
}
