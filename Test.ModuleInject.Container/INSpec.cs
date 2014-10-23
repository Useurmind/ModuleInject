using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test.ModuleInject.Container
{
    public interface INSpec
    {
        ActionRegister Context { get; set; }
        ActionRegister Describe { get; set; }
        ActionRegister It { get; set; }
        Action Todo { get; }
        ActionRegister XContext { get; set; }
        ActionRegister XDescribe { get; set; }
        ActionRegister XIt { get; set; }
        
        Action Act { get; set; }
        Action After { get; set; }
        Action AfterAll { get; set; }
        Action AfterEach { get; set; }
        Action Before { get; set; }
        Action BeforeAll { get; set; }
        Action BeforeEach { get; set; }
        Expression<Action> Specify { set; }
        Expression<Action> XSpecify { set; }

        Exception ExceptionToReturn(Exception originalException);
        Action expect<T>() where T : Exception;
        Action expect<T>(Action action) where T : Exception;
        Action expect<T>(string expectedMessage) where T : Exception;
        Action expect<T>(string expectedMessage, Action action) where T : Exception;
        string OnError(string flattenedStackTrace);
        string StackTraceToPrint(string flattenedStackTrace);
    }
}
