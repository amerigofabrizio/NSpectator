﻿using System;
using NSpectator;
using NSpectator.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using NSpectator.Specs.Running;
using FluentAssertions;

namespace NSpectator.Specs.Running.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class When_async_before_contains_exception : When_running_specs
    {
        class SpecClass : Spec
        {
            void method_level_context()
            {
                BeforeAsync = async () => 
                { 
                    await Task.Delay(0);
                    throw new BeforeException(); 
                };

                It["should fail this example because of beforeAsync"] = () => "1".Should().Be("1");

                It["should also fail this example because of beforeAsync"] = () => "1".Should().Be("1");

                It["overrides exception from same level it"] = () => { throw new ItException(); };

                Context["exception thrown by both beforeAsync and nested before"] = () =>
                {
                    Before = () => { throw new BeforeException(); };

                    It["overrides exception from nested before"] = () => "1".Should().Be("1");
                };

                Context["exception thrown by both beforeAsync and nested act"] = () =>
                {
                    Act = () => { throw new ActException(); };

                    It["overrides exception from nested act"] = () => "1".Should().Be("1");
                };

                Context["exception thrown by both beforeAsync and nested it"] = () =>
                {
                    It["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                Context["exception thrown by both beforeAsync and nested after"] = () =>
                {
                    It["overrides exception from nested after"] = () => "1".Should().Be("1");

                    After = () => { throw new AfterException(); };
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
            TheExample("should fail this example because of beforeAsync")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of beforeAsync")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from same level it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested before")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested act")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested after")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_async_before_failure_should_fail_because_of_async_before()
        {
            TheExample("should fail this example because of beforeAsync")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
            TheExample("should also fail this example because of beforeAsync")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_before()
        {
            TheExample("overrides exception from nested before")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }
    }
}
