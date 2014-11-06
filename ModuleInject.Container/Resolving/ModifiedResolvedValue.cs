using ModuleInject.Container.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Resolving
{
    public class ModifiedResolvedValue : IResolvedValue
    {
        private IResolvedValue baseResolvedValue;
        private List<Action<object>> actionModifications;

        public ModifiedResolvedValue(IResolvedValue baseResolvedValue, Action<object> modifyAction)
            : this(baseResolvedValue)
        {
            this.AddModification(modifyAction);
        }


        public ModifiedResolvedValue(IResolvedValue baseResolvedValue)
        {
            this.actionModifications = new List<Action<object>>();
            this.baseResolvedValue = baseResolvedValue;
        }

        public void AddModification(Action<object> modifyAction)
        {
            this.actionModifications.Add(modifyAction);
        }

        public object Resolve()
        {
            object instance = baseResolvedValue.Resolve();

            foreach (var modification in this.actionModifications)
            {
                modification(instance);
            }

            return instance;
        }

        public Type Type
        {
            get
            {
                return baseResolvedValue.Type;
            }
        }
    }
}
