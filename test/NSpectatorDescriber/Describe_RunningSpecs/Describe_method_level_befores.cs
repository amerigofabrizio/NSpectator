﻿using System;
using System.Linq;
using NSpectator;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpectator.Describer.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_befores : When_running_specs
    {
        class SpecClass : nspec
        {
            public static Action ContextLevelBefore = () => { };
            public static Action SubContextBefore = () => { };
            public static Func<Task> AsyncSubContextBefore = async () => { await Task.Delay(0); };

            // method- (or class-) level before
            void before_each() 
            { 
            }

            void method_level_context()
            {
                before = ContextLevelBefore;

                context["sub context"] = () => 
                {
                    before = SubContextBefore;

                    it["needs an example or it gets filtered"] = todo;
                };

                context["sub context with async before"] = () =>
                {
                    beforeAsync = AsyncSubContextBefore;

                    it["needs another example or it gets filtered"] = todo;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_set_method_level_before()
        {
            // Could not find a way to actually verify that deep inside 
            // 'BeforeInstance' there is a reference to 'SpecClass.before_each()'

            classContext.BeforeInstance.Should().NotBeNull();
        }

        [Test]
        [Category("Async")]
        public void it_should_not_set_async_method_level_before()
        {
            classContext.BeforeInstanceAsync.should_be_null();
        }

        [Test]
        public void it_should_set_before_on_method_level_context()
        {
            methodContext.Before.should_be(SpecClass.ContextLevelBefore);
        }

        [Test]
        public void it_should_set_before_on_sub_context()
        {
            methodContext.Contexts.First().Before.should_be(SpecClass.SubContextBefore);
        }

        [Test]
        [Category("Async")]
        public void it_should_set_async_before_on_sub_context()
        {
            methodContext.Contexts.Last().BeforeAsync.should_be(SpecClass.AsyncSubContextBefore);
        }
    }
}
