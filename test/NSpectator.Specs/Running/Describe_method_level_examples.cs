﻿#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System.Linq;
using NSpectator.Domain;
using NSpectator.Domain.Formatters;
using NUnit.Framework;
using System;
using Moq;
using FluentAssertions;

namespace NSpectator.Specs.Running
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class Describe_method_level_examples : Describe_method_level_examples_common_cases
    {
        class SpecClass : Spec
        {
            public static bool first_example_executed, last_example_executed;

            void it_should_be_an_example()
            {
                first_example_executed = true;
                "hello".Should().Be("hello");
            }

            void it_should_be_failing()
            {
                last_example_executed = true;
                "hello".Should().NotBe("hello");
            }
        }

        [SetUp]
        public void Setup()
        {
            RunWithReflector(typeof(SpecClass));
        }

        protected override bool FirstExampleExecuted => SpecClass.first_example_executed;

        protected override bool LastExampleExecuted => SpecClass.last_example_executed;
    }

    public abstract class Describe_method_level_examples_common_cases : When_running_method_level_examples
    {
        protected abstract bool FirstExampleExecuted { get; }
        protected abstract bool LastExampleExecuted { get; }

        [Test]
        public void the_class_context_should_contain_a_class_level_example()
        {
            classContext.Examples.Should().HaveCount(2);
        }

        [Test]
        public void there_should_be_only_one_failure()
        {
            classContext.Failures().Should().HaveCount(1);
        }

        [Test]
        public void should_execute_first_example()
        {
            FirstExampleExecuted.Should().BeTrue();
        }

        [Test]
        public void should_execute_last_example()
        {
            LastExampleExecuted.Should().BeTrue();
        }

        [Test]
        public void the_last_example_should_be_failing()
        {
            classContext.Examples.Last().Exception.Should().CastTo<AssertionException>();
        }

        [Test]
        public void the_stack_trace_for_last_example_should_be_the_the_original_stack_trace()
        {
            classContext.Examples.Last().Exception.StackTrace.Should().NotMatch("^.*at NSpectator.Domain.Example");
        }
    }

    public abstract class When_running_method_level_examples
    {
        protected void RunWithReflector(Type specClassType)
        {
            var reflectorMock = new Mock<IReflector>();
            reflectorMock.Setup(r => r.GetTypesFrom()).Returns(new[] { specClassType });
            
            var contextBuilder = new ContextBuilder(new SpecFinder(reflectorMock.Object), new DefaultConventions());

            classContext = contextBuilder.Contexts().First();
            classContext.Build();
            classContext.Run(new SilentLiveFormatter(), failFast: false);
        }

        protected Context classContext;
    }
}
