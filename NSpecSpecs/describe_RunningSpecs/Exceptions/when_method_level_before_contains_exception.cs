﻿using System;
using System.Linq;
using NSpectator;
using NSpectator.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_before_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void before_each()
            {
                throw new BeforeEachException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".should_be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .ShouldCastTo<ExampleFailureException>();
        }

        class BeforeEachException : Exception { }
    }
}
