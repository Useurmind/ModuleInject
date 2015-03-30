using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Performance.ModuleInject.Components
{
    public interface ITestComponent1
    {
        ITestComponent2 Part1 { get; }
        ITestComponent2 Part2 { get; }
        ITestComponent2 Part3 { get; }
    }

    public class TestComponent1 : ITestComponent1
    {
        public ITestComponent2 Part1 { get; set; }
        public ITestComponent2 Part2 { get; set; }
        public ITestComponent2 Part3 { get; set; }

        public TestComponent1()
        {

        }

        public TestComponent1(TestComponent2 part1)
        {
            Part1 = part1;
        }

        public TestComponent1(TestComponent2 part1, TestComponent2 part2, TestComponent2 part3)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
        }

        public void Inject(TestComponent2 part1)
        {
            Part1 = part1;
        }

        public void Inject(TestComponent2 part1, TestComponent2 part2, TestComponent2 part3)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
        }
    }
}
