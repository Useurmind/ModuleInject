﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IMainComponent1Sub
    {
        IMainComponent2 MainComponent2 { get; set; }
    }

    public interface IMainComponent1 : IMainComponent1Sub
    {
        int InjectedValue { get;  }
        IMainComponent2 MainComponent22 { get; set;  }
        IMainComponent2 MainComponent23 { get; }
        ISubComponent1 SubComponent1 { get; }

        IMainComponent1 RecursiveComponent1 { get; }
        IMainComponent1 RecursiveFactory1();

        IMainComponent2SubInterface ComponentViaSubinterface { get; }

        void Initialize(IMainComponent2 dependency1);

        int FunctionReturns5();
    }

    public class MainComponent1 : IMainComponent1
    {
        public int InjectedValue { get; set; }
        public IMainComponent2 MainComponent2 { get; set; }
        public IMainComponent2 MainComponent22 { get; set; }
        public IMainComponent2 MainComponent23 { get; set; }
        public ISubComponent1 SubComponent1 { get; set; }

        public IMainComponent1 RecursiveComponent1 { get; set; }

        public IMainComponent2SubInterface ComponentViaSubinterface { get; set; }

        public MainComponent1()
        {

        }

        public MainComponent1(IMainComponent2 mainComponent2)
        {
            MainComponent2 = mainComponent2;
        }

        public void CallWithConstant(IMainComponent2 dependency1, int asd)
        {
            MainComponent2 = dependency1;
            InjectedValue = asd;
        }

        public void Initialize(IMainComponent2 dependency1)
        {
            MainComponent2 = dependency1;
        }

        public void Initialize(ISubComponent1 dependency1)
        {
            SubComponent1 = dependency1;
        }

        public void Initialize(IMainComponent2 dependency1, ISubComponent1 dependency2)
        {
            MainComponent2 = dependency1;
            SubComponent1 = dependency2;
        }

        public void Initialize(IMainComponent2 dependency1, IMainComponent2 dependency2, ISubComponent1 dependency3)
        {
            MainComponent2 = dependency1;
            MainComponent22 = dependency2;
            SubComponent1 = dependency3;
        }

        public IMainComponent1 RecursiveFactory1() { return null; }

        public int FunctionReturns5()
        {
            return 5;
        }
    }
}
