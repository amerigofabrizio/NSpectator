﻿using System;
using System.Linq;
using NSpectator;
using NSpectator.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using NSpectator.Describer.WhenRunningSpecs;

namespace NSpectator.Describer.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class Describe_expected_exception : when_expecting_exception
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                it["throws expected exception"] = expect<KnownException>(() => { throw new KnownException(); });

                it["throws expected exception with expected error message"] = expect<KnownException>("Testing", () => { throw new KnownException("Testing"); });

                it["fails if expected exception does not throw"] = expect<KnownException>(() => { });

                it["fails if wrong exception thrown"] = expect<KnownException>(() => { throw new SomeOtherException(); });

                it["fails if wrong error message is returned"] = expect<KnownException>("Testing", () => { throw new KnownException("Blah"); });
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
    public class Describe_async_expected_exception_before_awaiting_a_task : when_expecting_exception
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                itAsync["throws expected exception"] = expectAsync<KnownException>(async () =>
                    await Task.Run(() =>
                    {
                        throw new KnownException();
                    }));

                itAsync["throws expected exception with expected error message"] = expectAsync<KnownException>("Testing", async () =>
                    await Task.Run(() => { throw new KnownException("Testing"); }));

                itAsync["fails if expected exception does not throw"] = expectAsync<KnownException>(async () =>
                    await Task.Run(() => { }));

                itAsync["fails if wrong exception thrown"] = expectAsync<KnownException>(async () =>
                    await Task.Run(() => { throw new SomeOtherException(); }));

                itAsync["fails if wrong error message is returned"] = expectAsync<KnownException>("Testing", async () =>
                    await Task.Run(() => { throw new KnownException("Blah"); }));
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
    public class Describe_async_expected_exception_after_awaiting_a_task : when_expecting_exception
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                itAsync["throws expected exception"] = expectAsync<KnownException>(async () =>
                {
                    await Task.Run(() => { });

                    throw new KnownException();
                });

                itAsync["throws expected exception with expected error message"] = expectAsync<KnownException>("Testing", async () =>
                {
                    await Task.Run(() => { } );

                    throw new KnownException("Testing");
                });

                itAsync["fails if expected exception does not throw"] = expectAsync<KnownException>(async () =>
                    await Task.Run(() => { }));

                itAsync["fails if wrong exception thrown"] = expectAsync<KnownException>(async () =>
                {
                    await Task.Run(() => { } );

                    throw new SomeOtherException();
                });

                itAsync["fails if wrong error message is returned"] = expectAsync<KnownException>("Testing", async () =>
                {
                    await Task.Run(() => {  } );

                    throw new KnownException("Blah");
                });
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    public abstract class when_expecting_exception : When_running_specs
    {
        [Test]
        public void should_be_three_failures()
        {
            classContext.Failures().Count().should_be(3);
        }

        [Test]
        public void throws_expected_exception()
        {
            TheExample("throws expected exception").should_have_passed();
        }

        [Test]
        public void throws_expected_exception_with_error_message_Testing()
        {
            TheExample("throws expected exception with expected error message").should_have_passed();
        }

        [Test]
        public void fails_if_expected_exception_not_thrown()
        {
            TheExample("fails if expected exception does not throw").Exception.GetType().should_be(typeof(ExceptionNotThrown));
        }

        [Test]
        public void fails_if_wrong_exception_thrown()
        {
            var exception = TheExample("fails if wrong exception thrown").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Exception of type KnownException was not thrown.");
        }

        [Test]
        public void fails_if_wrong_error_message_is_returned()
        {
            var exception = TheExample("fails if wrong error message is returned").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Expected message: \"Testing\" But was: \"Blah\"");
        }
    }
}