﻿using System;
using NSpectator;
using NSpectator.Domain;
using NSpectator.Describer.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpectator.Describer.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_after_all_contains_exception : When_running_specs
    {
        class SpecClass : Spec
        {
            void method_level_context()
            {
                afterAll = () => { throw new AfterAllException(); };

                it["should fail this example because of afterAll"] = () => "1".should_be("1");

                it["should also fail this example because of afterAll"] = () => "1".should_be("1");

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both afterAll and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => "1".should_be("1");
                };

                context["exception thrown by both afterAll and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["preserves exception from nested act"] = () => "1".should_be("1");
                };

                context["exception thrown by both afterAll and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both afterAll and nested after"] = () =>
                {
                    it["preserves exception from nested after"] = () => "1".should_be("1");

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of afterAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("overrides exception from same level it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("preserves exception from nested before")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("preserves exception from nested act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("preserves exception from nested after")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_after_all_failure_should_fail_because_of_after_all()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
            TheExample("should also fail this example because of afterAll")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
        }

        [Test]
        public void it_should_throw_exception_from_after_all_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_after_all()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.GetType().should_be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_nested_act_not_from_after_all()
        {
            TheExample("preserves exception from nested act")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_after_all_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
        }

        [Test]
        public void it_should_throw_exception_from_nested_after_not_from_after_all()
        {
            TheExample("preserves exception from nested after")
                .Exception.InnerException.GetType().should_be(typeof(AfterException));
        }
    }
}
