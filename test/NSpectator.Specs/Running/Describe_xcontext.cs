﻿#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace NSpectator.Specs.Running
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class Describe_it_behaviour_in_xcontext : When_running_specs
    {
        class SpecClass : Spec
        {
            void method_level_context()
            {
                xContext["sub context"] = () =>
                {
                    It["needs an example or it gets filtered"] =
                        () => "Hello World".Should().Be("Hello World");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_should_be_pending()
        {
            methodContext.Contexts.First().Examples.First().Pending.Should().Be(true);
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class Describe_xcontext : When_running_specs
    {
        class SpecClass : Spec
        {
            public static string output = string.Empty;
            public static Action MethodLevelBefore = () => { throw new Exception("this should not run."); };
            public static Action SubContextBefore = () => { throw new Exception("this should not run."); };

            void method_level_context()
            {
                Before = MethodLevelBefore;

                xContext["sub context"] = () =>
                {
                    Before = SubContextBefore;

                    It["needs an example or it gets filtered"] =
                        () => "Hello World".Should().Be("Hello World");
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_not_run_befores_on_pending_context()
        {
            methodContext.AllExamples().First().Exception.Should().Be(null);
        }
    }
}
