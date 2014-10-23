using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container
{
    using global::NSpec;

    public class NSpecBase : nspec, INSpec
    {
        public global::NSpec.Domain.ActionRegister Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
            }
        }

        public global::NSpec.Domain.ActionRegister Describe
        {
            get
            {
                return this.describe;
            }
            set
            {
                this.describe = value;
            }
        }

        public global::NSpec.Domain.ActionRegister It
        {
            get
            {
                return it;
            }
            set
            {
                it = value;
            }
        }

        public Action Todo
        {
            get
            {
                return todo;
            }
        }

        public global::NSpec.Domain.ActionRegister XContext
        {
            get
            {
                return xcontext;
            }
            set
            {
                xcontext = value;
            }
        }

        public global::NSpec.Domain.ActionRegister XDescribe
        {
            get
            {
                return xdescribe;
            }
            set
            {
                xdescribe = value;
            }
        }

        public global::NSpec.Domain.ActionRegister XIt
        {
            get
            {
                return xit;
            }
            set
            {
                xit = value;
            }
        }

        public Action Act
        {
            get
            {
                return act;
            }
            set
            {
                act = value;
            }
        }

        public Action After
        {
            get
            {
                return after;
            }
            set
            {
                after = value;

            }
        }

        public Action AfterAll
        {
            get
            {
                return afterAll;
            }
            set
            {
                afterAll = value;
            }
        }

        public Action AfterEach
        {
            get
            {
                return afterEach;
            }
            set
            {
                afterEach = value;
            }
        }

        public Action Before
        {
            get
            {
                return before;
            }
            set
            {
                before = value;
            }
        }

        public Action BeforeAll
        {
            get
            {
                return beforeAll;
            }
            set
            {
                beforeAll = value;
            }
        }

        public Action BeforeEach
        {
            get
            {
                return beforeEach;
            }
            set
            {
                beforeEach = value;
            }
        }

        public System.Linq.Expressions.Expression<Action> Specify
        {
            set
            {
                specify = value;
            }
        }

        public System.Linq.Expressions.Expression<Action> XSpecify
        {
            set
            {
                xspecify = value;
            }
        }
    }
}
