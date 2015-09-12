using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Test.Performance.ModuleInject
{
    class TimeMeasure
    {
        private Action measuredAction;
        private int repetitions;

        public double MeasuredTotalTimeMs { get; private set; }
        public double MeasuredAverageTimeMs
        {
            get
            {
                return MeasuredTotalTimeMs / (double)repetitions;
            }
        }

        public TimeMeasure(Action measuredAction, int repetitions=10000)
        {
            this.measuredAction = measuredAction;
            this.repetitions = repetitions;
        }

        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < repetitions; i++)
            {
                measuredAction();
            }
            stopwatch.Stop();

            MeasuredTotalTimeMs = stopwatch.ElapsedMilliseconds;
        }
    }
}
