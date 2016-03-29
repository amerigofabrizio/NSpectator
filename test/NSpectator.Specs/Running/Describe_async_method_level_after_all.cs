﻿using NSpectator;
using NSpectator.Domain;
using NSpectator.Specs.Running;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpectator.Specs.Running
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_method_level_after_all : When_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void it_should_have_initial_value()
            {
                ShouldHaveInitialState();
            }

            async Task after_all()
            {
                await SetStateAsync();
            }
        }

        [Test]
        public void async_method_level_after_all_waits_for_task_to_complete()
        {
            Run(typeof(SpecClass));

            ExampleRunsWithExpectedState("it should have initial value");
        }

        class WrongSpecClass : BaseSpecClass
        {
            void it_should_not_know_what_to_do()
            {
                PassAlways();
            }

            void after_all()
            {
                SetAnotherState();
            }

            async Task after_all_async()
            {
                await SetStateAsync();
            }
        }

        [Test]
        public void class_with_both_sync_and_async_after_all_always_fails()
        {
            Run(typeof(WrongSpecClass));

            ExampleRunsWithException("it should not know what to do");
        }
    }
}
