﻿#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace NSpectator.Specs.Running
{
    [TestFixture]
    class Describe_examples_for_abstract_class : When_running_specs
    {
        class Base : Spec
        {
            protected List<int> ints;
            
            void before_each()
            {
                ints = new List<int> { 1 };
            }

            void list_manipulations()
            {
                It["should be 1"] = () => ints.Should().Equal(1);
            }
        }

        abstract class Abstract : Base
        {
            void before_each()
            {
                ints.Add(2);
            }

            void list_manipulations()
            {
                //since abstract classes can only run in derived concrete context classes
                //the context isn't quite what you might expect.
                It["should be 1, 2, 3"] = () => ints.Should().Equal(1, 2, 3);
            }
        }

        class Concrete : Abstract
        {
            void before_each()
            {
                ints.Add(3);
            }

            void list_manipulations()
            {
                It["should be 1, 2, 3 too"] = () => ints.Should().Equal(1, 2, 3);
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(Concrete));
        }

        [Test]
        public void should_run_example_within_a_sub_context_in_a_derived_class()
        {
            TheExample("should be 1").Should().HavePassed();
        }

        [Test]
        public void it_runs_examples_from_abstract_class_as_if_they_belonged_to_concrete_class()
        {
            TheExample("should be 1, 2, 3").Should().HavePassed();

            TheExample("should be 1, 2, 3 too").Should().HavePassed();
        }
    }
}
