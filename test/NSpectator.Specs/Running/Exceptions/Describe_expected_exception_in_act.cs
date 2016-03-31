﻿#region [R# naming]
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable InconsistentNaming
#endregion
using System.Linq;
using NSpectator.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpectator.Specs.Running.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class Describe_expected_exception_in_act : When_expecting_exception_in_act
    {
        private class SpecClass : Spec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<KnownException>();

                context["when exception thrown from act"] = () =>
                {
                    act = () => { throw new KnownException("Testing"); };

                    it["threw the expected exception in act"] = expect<KnownException>();

                    it["threw the exception in act with expected error message"] = expect<KnownException>("Testing");

                    it["fails if wrong exception thrown"] = expect<SomeOtherException>();

                    it["fails if wrong error message is returned"] = expect<KnownException>("Blah");
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(SpecClass));
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class Describe_expected_exception_in_async_act_before_awaiting_a_task : When_expecting_exception_in_act
    {
        private class SpecClass : Spec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<KnownException>();

                context["when exception thrown from act"] = () =>
                {
                    actAsync = async () => await Task.Run(() => { throw new KnownException("Testing"); });

                    it["threw the expected exception in act"] = expect<KnownException>();

                    it["threw the exception in act with expected error message"] = expect<KnownException>("Testing");

                    it["fails if wrong exception thrown"] = expect<SomeOtherException>();

                    it["fails if wrong error message is returned"] = expect<KnownException>("Blah");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class Describe_expected_exception_in_async_act_after_awaiting_a_task : When_expecting_exception_in_act
    {
        private class SpecClass : Spec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<KnownException>();

                context["when exception thrown from act after awaiting another task"] = () =>
                {
                    actAsync = async () =>
                    {
                        await Task.Run(() => { } );

                        throw new KnownException("Testing");
                    };

                    it["threw the expected exception in act"] = expect<KnownException>();

                    it["threw the exception in act with expected error message"] = expect<KnownException>("Testing");

                    it["fails if wrong exception thrown"] = expect<SomeOtherException>();

                    it["fails if wrong error message is returned"] = expect<KnownException>("Blah");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_expected_exception_in_async_act_within_list_of_tasks : When_expecting_exception_in_act
    {
        private class SpecClass : Spec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<KnownException>();

                context["when exception thrown from act within a list of tasks"] = () =>
                {
                    actAsync = () =>
                    {
                        var tasks = Enumerable.Range(0, 10)
                            .Select(e => Task.Run(() =>
                            {
                                if (e == 4)
                                {
                                    throw new KnownException("Testing");
                                }
                            }));

                        return Task.WhenAll(tasks);
                    };

                    it["threw the expected exception in act"] = expect<KnownException>();

                    it["threw the exception in act with expected error message"] = expect<KnownException>("Testing");

                    it["fails if wrong exception thrown"] = expect<SomeOtherException>();

                    it["fails if wrong error message is returned"] = expect<KnownException>("Blah");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    public abstract class When_expecting_exception_in_act : When_running_specs
    {
        [Test]
        public void should_be_three_failures()
        {
            classContext.Failures().Count().Expected().ToBe(3);
        }

        [Test]
        public void threw_expected_exception_in_act()
        {
            TheExample("threw the expected exception in act").Should_have_passed();
        }

        [Test]
        public void threw_the_exception_in_act_with_the_proper_error_message()
        {
            TheExample("threw the exception in act with expected error message").Should_have_passed();
        }

        [Test]
        public void fails_if_no_exception_thrown()
        {
            TheExample("fails if no exception thrown").Exception.Should().BeOfType<ExceptionNotThrown>();
        }

        [Test]
        public void fails_if_wrong_exception_thrown()
        {
            var exception = TheExample("fails if wrong exception thrown").Exception;

            exception.Should().BeOfType<ExceptionNotThrown>();
            exception.Message.Should().Be("Exception of type SomeOtherException was not thrown.");
        }

        [Test]
        public void fails_if_wrong_error_message_is_returned()
        {
            var exception = TheExample("fails if wrong error message is returned").Exception;

            exception.Should().BeOfType<ExceptionNotThrown>();
            exception.Message.Should().Be("Expected message: \"Blah\" But was: \"Testing\"");
        }
    }
}