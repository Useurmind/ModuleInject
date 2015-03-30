using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Test.Performance.ModuleInject
{
    class Program
    {
        static void Main(string[] args)
        {
            TestModuleType<TestInjectionModule>();
            TestModuleType<TestManualModule>();

            Console.ReadLine();
        }

        private static void TestModuleType<TModule>()
            where TModule : class, IModule, new()
        {
            int repetitions = 10000;
            TModule module = null;

            var constructMeasure = new TimeMeasure(() =>
            {
                module = new TModule();
            });

            constructMeasure.Run();

            Console.WriteLine("Measured construction of module type '{0}' - total/average = {1}/{2}",
                typeof(TModule).Name,
                constructMeasure.MeasuredTotalTimeMs,
                constructMeasure.MeasuredAverageTimeMs);
        }
    }
}
