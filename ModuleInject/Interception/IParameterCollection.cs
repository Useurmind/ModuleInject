using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Interception
{
    public interface IParameterCollection : IList, ICollection, IEnumerable
    {
        object this[string parameterName] { get; set; }
        bool ContainsParameter(string parameterName);
        ParameterInfo GetParameterInfo(int index);
        ParameterInfo GetParameterInfo(string parameterName);
        string ParameterName(int index);
    }
}
