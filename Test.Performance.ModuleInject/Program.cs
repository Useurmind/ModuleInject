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
            TestModuleType<TestInjectionModuleV2>();
			TestModuleType<TestInjectionModuleV2NamedSourcesExpressions>();
			TestModuleType<TestInjectionModuleV2NamedSourcesWithStrings>();
			TestModuleType<TestInjectionModuleV2NamedSourcesMixed>();
			TestModuleType<TestInjectionModuleV2NamedSourcesAllInGet>();
			TestModuleType<TestManualModule>();
            TestModuleType<TestAutofacModule>();
            TestModuleType<TestUnityModule>();

            Console.ReadLine();
        }

        private static void TestModuleType<TModule>()
            where TModule : class, IModule, ITestModule, new()
        {
            int repetitions = 10000;
            TModule module = null;

            var constructMeasure = new TimeMeasure(() =>
            {
                module = new TModule();
			}, repetitions);

            constructMeasure.Run();

			var resolveMeasure = new TimeMeasure(() =>
			{
				module = new TModule();

				module.Resolve();

				var c1 = module.Component1;
				var c2 = module.Component2;
				var c3 = module.Component3;
				var c4 = module.Component4;
				var c5 = module.Component5;
				var c6 = module.Component6;
				var c7 = module.Component7;
				var c8 = module.Component8;
				var c9 = module.Component9;
				var c10 = module.Component10;
			}, repetitions);

			resolveMeasure.Run();

			Console.WriteLine("Measured time for module type '{0}': resolve (construct): {1} ({2})",
                typeof(TModule).Name,
				resolveMeasure.MeasuredTotalTimeMs,
                constructMeasure.MeasuredTotalTimeMs);
        }
    }
}
