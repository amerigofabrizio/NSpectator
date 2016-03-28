﻿using System.Collections.Generic;
using System.Linq;
using NSpectator;
using NSpectator.Domain;
using NUnit.Framework;

namespace NSpectator.Specs.Running
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_implicit_befores : When_running_specs
    {
        class SpecClass : Spec
        {
            void method_level_context()
            {
                List<int> ints = new List<int>();
                ints.Add(5);

                it["should have two entries"] = () =>
                {
                    ints.Add(16);
                    ints.Count.should_be(1);
                };

                specify = () => ints.Count.should_be(1);
            }
        }

        [Test, Ignore("It cannot be tested")]
        public void should_give_each_specify_a_new_instance_of_spec()
        {
            Run(typeof(SpecClass));
            Assert.Inconclusive("I dont think this is possible....");
            TheMethodContextExamples().First().should_have_passed();
            TheMethodContextExamples().Last().should_have_passed();
        }

        private IEnumerable<ExampleBase> TheMethodContextExamples()
        {
            return classContext.Contexts.First().AllExamples();
        }
    }
}
